using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Provider
{
    public class StreamBodyProvider : DefaultBodyProvider
    {
        private string contentType;
        private Stream body;

        public StreamBodyProvider(Stream body) : this("application/octet-stream", body)
        {
            
        }

        public StreamBodyProvider(string contentType, Stream body)
        {
            this.body = body;
            this.contentType = contentType;
        }

        public override string GetContentType()
        {
            return contentType;
        }

        public override Stream GetBody()
        {
            return body;
        }
    }
}
