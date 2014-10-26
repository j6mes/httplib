namespace JumpKick.HttpLib
{
    using JumpKick.HttpLib.Provider;
    using System;
    using System.IO;
    using System.Net;


    public class Request
    {
        private string url;
        private HttpVerb method;
        private HeaderProvider headers;
        private AuthenticationProvider auth;
        private BodyProvider body;
        private static CookieContainer cookies = new CookieContainer();
        private Action<WebHeaderCollection, Stream> successCallback;
        private Action<WebException> failCallback;

        public void Go()
        {

        }


        private void MakeRequest()
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("url is empty");
            }


            try
            {
                /*
                 * Create new Request
                 */
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
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


        private static AsyncCallback ProcessCallback(Action<WebHeaderCollection, Stream> success, Action<WebException> fail)
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
