namespace JumpKick.HttpLib
{
    using JumpKick.HttpLib.Provider;
    using System;
    using System.IO;
    using System.Net;


    public class Request
    {
        protected string url;
        protected HttpVerb method = HttpVerb.Get;
        protected HeaderProvider headers;
        protected AuthenticationProvider auth;
        protected BodyProvider body;
        protected static CookieContainer cookies = new CookieContainer();
        protected Action<WebHeaderCollection, Stream> successCallback;
        protected Action<WebException> failCallback;

        public void Go()
        {

        }

        public HttpVerb GetMethod()
        {
            return method;
        }

        protected HttpWebRequest GetWebRequest(string url)
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
                request.CookieContainer = cookies;
                request.Method = method.ToString().ToUpper();

                if (method == HttpVerb.Get || method == HttpVerb.Head) 
                {
                    request.BeginGetResponse(ProcessCallback(successCallback, failCallback), request);
                } 
                else 
                {
                    request.ContentType = body.GetContentType(); ;
                    request.BeginGetRequestStream(new AsyncCallback((IAsyncResult callbackResult) =>
                    {
                        HttpWebRequest tmprequest = (HttpWebRequest)callbackResult.AsyncState;
                        body.GetBody().CopyTo(tmprequest.EndGetRequestStream(callbackResult));

                        // Start the asynchronous operation to get the response
                        tmprequest.BeginGetResponse(ProcessCallback(successCallback, failCallback), tmprequest);


                    }), request);
                }
            }
            catch (WebException webEx)
            {
                failCallback(webEx);
            }
        }


        protected AsyncCallback ProcessCallback(Action<WebHeaderCollection, Stream> success, Action<WebException> fail)
        {
            return new AsyncCallback((callbackResult) =>
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;

                try
                {
                    using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult))
                    {
                        success(myResponse.Headers, myResponse.GetResponseStream());
                    }

                }
                catch (WebException webEx)
                {
                    fail(webEx);
                }
            });
        }
    }

   
}
