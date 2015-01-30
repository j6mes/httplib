
namespace JumpKick.HttpLib.Provider
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class TextBodyProvider : DefaultBodyProvider
    {
        private Stream contentstream;
        private StreamWriter writer;

        private String contentType;

        public TextBodyProvider(String text) : this("application/text",text)
        {
        }


        public TextBodyProvider(String contentType, String text)
        {
            contentstream = new MemoryStream();
            writer = new StreamWriter(contentstream);

            this.contentType = contentType;

            writer.Write(text);
            writer.Flush();
        }


        public override string GetContentType()
        {
            return this.contentType;
        }

        public override Stream GetBody()
        {
            contentstream.Seek(0,SeekOrigin.Begin);
            return contentstream;
        }

    }
}
