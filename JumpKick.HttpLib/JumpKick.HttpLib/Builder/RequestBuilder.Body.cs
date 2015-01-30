using JumpKick.HttpLib.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Builder
{
    public partial class RequestBuilder
    {
        #region Body
        private BodyProvider bodyProvider;

        public RequestBuilder Upload(NamedFileStream[] files, object parameters, Action<long,long?> onProgressChanged, Action<long> onUploadComplete)
        {
            MultipartBodyProvider bodyProvider = new MultipartBodyProvider();
            bodyProvider.OnCompletedCallback = onUploadComplete;
            bodyProvider.OnProgressChangeCallback = onProgressChanged;
            foreach (NamedFileStream file in files)
            {
                bodyProvider.AddFile(file);
            }

            bodyProvider.SetParameters(parameters);
            return this.Body(bodyProvider);
        }

        public RequestBuilder Upload(NamedFileStream[] files, object parameters)
        {
            return this.Upload(files, parameters,(a,b)=> { }, a=>{ });
        }

        public RequestBuilder Upload(NamedFileStream[] files)
        {
            return this.Upload(files, new { });
        }

        public RequestBuilder Upload(NamedFileStream[] files, Action<long, long?> onProgressChanged)
        {
            return this.Upload(files, new { }, onProgressChanged, a => { });
        }


        public RequestBuilder Form(object body)
        {
            FormBodyProvider bodyProvider = new FormBodyProvider();
            bodyProvider.AddParameters(body);

            return this.Body(bodyProvider);
        }


        public RequestBuilder Form(IDictionary<String, String> body)
        {
            FormBodyProvider bodyProvider = new FormBodyProvider();
            bodyProvider.AddParameters(body);

            return this.Body(bodyProvider);
        }

        public RequestBuilder Body(Stream stream)
        {
            return this.Body(new StreamBodyProvider(stream));
        }

        public RequestBuilder Body(String contentType, Stream stream)
        {
            return this.Body(new StreamBodyProvider(contentType, stream));
        }


        public RequestBuilder Body(String text)
        {
            return this.Body(new TextBodyProvider(text));
        }

        public RequestBuilder Body(String contentType, String text)
        {
            return this.Body(new TextBodyProvider(contentType, text));
        }



        public RequestBuilder Body(BodyProvider provider)
        {
            if (this.method == HttpVerb.Head || this.method == HttpVerb.Get)
            {
                throw new InvalidOperationException("Cannot set the body of a GET or HEAD request");
            }

            this.bodyProvider = provider;
            return this;
        }

        #endregion
    }
}
