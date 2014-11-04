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
    public class RequestBuilder
    {
        private string url;
        private HttpVerb method;

        public RequestBuilder(string url, HttpVerb method)
        {
            this.url = url;
            this.method = method;
        }

        #region Headers
        private HeaderProvider headerProvider;

        public RequestBuilder Headers(object header)
        {
            Dictionary<String, String> headers = new Dictionary<String, String>();
            PropertyInfo[] properties;
#if NETFX_CORE
            properties = header.GetType().GetTypeInfo().DeclaredProperties.ToArray();
#else
            properties = header.GetType().GetProperties();
#endif

            foreach (var property in properties)
            {
                headers.Add(property.Name, System.Uri.EscapeDataString(property.GetValue(header, null).ToString()));
            }
            headerProvider = new DictionaryHeaderProvider(headers);
            return this;
        }

        public RequestBuilder Headers(IDictionary<String, String> header)
        {
            this.headerProvider = new DictionaryHeaderProvider(header);
            return this;
        }

        public RequestBuilder Headers(HeaderProvider headerProvider)
        {
            this.headerProvider = headerProvider;
            return this;
        }

        #endregion

        #region Auth
        private AuthenticationProvider authProvider;
        public RequestBuilder Auth(string username, string password)
        {
            authProvider = new BasicAuthenticationProvider(username, password);
            return this;
        }

        public RequestBuilder Auth(string text)
        {
            authProvider = new TextAuthenticationProvider(text);
            return this;
        }

        public RequestBuilder Auth(AuthenticationProvider provider)
        {
            authProvider = provider;
            return this;
        }
        #endregion

        #region Body
        private BodyProvider bodyProvider;

        public RequestBuilder Upload(NamedFileStream[] files, object parameters) 
        {
            MultipartBodyProvider bodyProvider = new MultipartBodyProvider();
            
            foreach (NamedFileStream file in files)
            {
                bodyProvider.AddFile(file);
            }

            bodyProvider.SetParameters(parameters);
            this.bodyProvider = bodyProvider;
            return this;
        }


        public RequestBuilder Upload(NamedFileStream[] files)
        {
            return this.Upload(files, new { });
        }

        public RequestBuilder Form(object body)
        {
            FormBodyProvider bodyProvider = new FormBodyProvider();
            bodyProvider.AddParameters(body);

            this.bodyProvider = bodyProvider;
            return this;
        }


        public RequestBuilder Form(IDictionary<String, String> body)
        {
            FormBodyProvider bodyProvider = new FormBodyProvider();
            bodyProvider.AddParameters(body);

            this.bodyProvider = bodyProvider;
            return this;
        }

        public RequestBuilder Body(Stream stream)
        {
            this.bodyProvider = new StreamBodyProvider(stream);
            return this;
        }

        public RequestBuilder Body(String contentType, Stream stream)
        {
            this.bodyProvider = new StreamBodyProvider(contentType, stream);
            return this;
        }


        public RequestBuilder Body(String text)
        {
            this.bodyProvider = new TextBodyProvider(text);
            return this;
        }

        public RequestBuilder Body(String contentType, String text)
        {
            this.bodyProvider = new TextBodyProvider(contentType, text);
            return this;
        }



        public RequestBuilder Body(BodyProvider provider)
        {
            this.bodyProvider = provider;
            return this;
        }

        #endregion

        #region Actions
        ActionProvider actionProvider;
        Action<WebHeaderCollection, Stream> success;
        Action<WebException> fail;
        public RequestBuilder OnSuccess(Action<WebHeaderCollection, String> action)
        {
            this.success = (headers, stream) =>
            {
                StreamReader reader = new StreamReader(stream);
                action(headers, reader.ReadToEnd());
            };


            return this;
        }

        public RequestBuilder OnSuccess(Action<String> action)
        {
            this.success = (headers, stream) =>
                {
                    StreamReader reader = new StreamReader(stream);
                    action(reader.ReadToEnd());
                };
            return this;
        }


        public RequestBuilder OnSuccess(Action<WebHeaderCollection, Stream> action)
        {        
            return this;
        }

        public RequestBuilder OnFail(Action<WebException> action)
        {
            fail = action;
            return this;
        }


        public RequestBuilder Action(ActionProvider action)
        {
            actionProvider = action;
            return this;
        }

        #endregion


        
        public void Go()
        { 
            
        }




    }
}
