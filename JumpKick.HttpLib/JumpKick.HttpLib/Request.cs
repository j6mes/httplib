namespace JumpKick.HttpLib
{
    using JumpKick.HttpLib.Provider;
    using System;
    using System.IO;
    using System.Net;
    using JumpKick.HttpLib.Streams;
    using System.Threading;
    using System.Threading.Tasks;

    public class Request
    {
        protected string url;
        protected HttpVerb method = HttpVerb.Get;
        protected HeaderProvider headers;
        protected AuthenticationProvider auth;
        protected BodyProvider body;

        protected ActionProvider action;

#if USE_SEMAPHORE_SLIM
        /// <summary>
        /// Using for GoAsync() to await.
        /// </summary>
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
#endif

        public Request()
        {
            
        }


        public String Url
        {
            set
            {
                this.url = value;
            }
            get
            {
                return this.url;
            }
        }

        public HttpVerb Method
        {
            set
            {
                this.method = value;
            }

            get
            {
                return this.method;
            }
        }

        public HeaderProvider Headers
        {
            set
            {
                this.headers = value;
            }
            get
            {
                return this.headers;
            }
        }

        public AuthenticationProvider Auth
        {
            set
            {
                this.auth = value;
            }
            get
            {
                return this.auth;
            }
        }

        public ActionProvider Action
        {
            set
            {
                this.action = value;
            }
            get
            {
                return this.action;
            }
        }

        public BodyProvider Body
        {
            set
            {
                this.body = value;
            }
            get
            {
                return this.body;
            }
        }

        public void Go()
        {
            MakeRequest();
        }

#if USE_SEMAPHORE_SLIM
        public async Task GoAsync(int millisecondsTimeout)
        {
            MakeRequest();
            await _semaphore.WaitAsync(millisecondsTimeout);
        }
#endif

        protected virtual HttpWebRequest GetWebRequest(string url)
        {
            return (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        }

        protected void MakeRequest()
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url is empty");
            }

            try
            {
                /*
                 * Create new Request
                 */
                HttpWebRequest request = this.GetWebRequest(url);
                request.CookieContainer = Cookies.Container;
                request.Method = method.ToString().ToUpper();
                if (action != null && action.Make != null)
                    action.Make(request); //Pass the request out

                if (headers != null)
                {
                    request.Headers = GetHeadersFromProvider(request.RequestUri, headers.GetHeaders());
                }

                if (method == HttpVerb.Get || method == HttpVerb.Head || body == null) 
                {
                    ExecuteRequestWithoutBody(request);
                } 
                else 
                {
                    request.ContentType = body.GetContentType();
                    ExecuteRequestWithBody(request);
                }
            }
            catch (WebException webEx)
            {
#if USE_SEMAPHORE_SLIM
                _semaphore.Release();//for GoAsync()
#endif          
                if (action != null && action.Make != null)     
                    action.Fail(webEx);
                else
                    throw webEx;//better then a null Exception
            }
        }

        private static WebHeaderCollection GetHeadersFromProvider(Uri uri, Header[] headers) 
        {
            WebHeaderCollection whc = new WebHeaderCollection();

            foreach (Header h in headers) 
            {
                if (h.Name.ToUpperInvariant() == "COOKIE") 
                {
                    var cookiePairs = h.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cookiePair in cookiePairs)
                    {
                        var index = cookiePair.IndexOf('=');

                        Cookies.Container.Add(uri, new Cookie(cookiePair.Substring(0, index), cookiePair.Substring(index + 1, cookiePair.Length - index - 1)) { Domain = uri.Host });
                    }
                } 
                else 
                {
                    whc[h.Name] = h.Value;
                }
            }

            return whc;
        }


        protected virtual void ExecuteRequestWithoutBody(HttpWebRequest request)
        {
            request.BeginGetResponse(ProcessCallback(action.Success, action.Fail), request);
        }

        protected virtual void ExecuteRequestWithBody(HttpWebRequest request)
        {
            request.BeginGetRequestStream(new AsyncCallback((IAsyncResult callbackResult) =>
            {
                HttpWebRequest tmprequest = (HttpWebRequest)callbackResult.AsyncState;
                
      
                ProgressCallbackHelper copy = body.GetBody().CopyToProgress(tmprequest.EndGetRequestStream(callbackResult),null);
                copy.ProgressChanged += (bytesSent, totalBytes) => { body.OnProgressChange(bytesSent, totalBytes); };
                copy.Completed += (totalBytes) => { body.OnCompleted(totalBytes); };
                copy.Go();

                // Start the asynchronous operation to get the response
                tmprequest.BeginGetResponse(ProcessCallback(action.Success, action.Fail), tmprequest);


            }), request);
        }


        protected AsyncCallback ProcessCallback(Action<WebHeaderCollection, Stream> success, Action<WebException> fail)
        {
            return new AsyncCallback((callbackResult) =>
            {
                HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)webRequest.EndGetResponse(callbackResult))
                    {
                       // if (response.ContentLength > 0) { response.Headers.Add("Content-Length", response.ContentLength.ToString()); }
                        if (success!=null )
                            success(response.Headers, response.GetResponseStream());
                    }

                }
                catch (WebException webEx)
                {
                    if (fail != null)   
                        fail(webEx);
                    else
                        throw webEx;//better then a null Exception
                }
                finally
                {
#if USE_SEMAPHORE_SLIM
                    _semaphore.Release();//for GoAsync()
#endif
                }               
            });
        }
    }

   
}
