using JumpKick.HttpLib.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Builder
{
    public partial class RequestBuilder
    {
        private string url;
        private HttpVerb method;

        public RequestBuilder(string url, HttpVerb method)
        {
            this.url = url;
            this.method = method;
        }



        public void Go()
        { 
            /*
             * If an actionprovider has not been set, we create one.
             */
            if(this.actionProvider == null)
            {
                this.actionProvider = new SettableActionProvider(success, fail);
            }

            Request req = new Request
            {
                Url = url,
                Method = method,
                Action = actionProvider,
                Auth = authProvider,
                Headers = headerProvider,
                Body = bodyProvider
            };

            req.Go();
            
        }





        public HttpVerb Method { get { return method; } }
        public String Url { get { return url; } }



        public Action<WebHeaderCollection,Stream> GetOnSuccess()
        {
            return this.success;
        }
    }
}
