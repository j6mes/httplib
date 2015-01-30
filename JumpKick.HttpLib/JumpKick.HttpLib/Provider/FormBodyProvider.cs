
namespace JumpKick.HttpLib.Provider
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class FormBodyProvider : DefaultBodyProvider
    {
        private Stream contentstream;
        private StreamWriter writer;
        public FormBodyProvider()
        {
            contentstream = new MemoryStream();
            writer = new StreamWriter(contentstream);
        }


        public override string GetContentType()
        {
            return "application/x-www-form-urlencoded";
        }

        public override Stream GetBody()
        {
            contentstream.Seek(0,SeekOrigin.Begin);
            return contentstream;
        }

        public void AddParameters(object parameters)
        {
            writer.Write(SerializeQueryString(parameters));
            writer.Flush();
        }

        public void AddParameters(IDictionary<String,String> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters cannot be null");
            }


            int i = 0;
            foreach (var property in parameters)
            {
                writer.Write(property.Key + "=" + System.Uri.EscapeDataString(property.Value));

                if (++i < parameters.Count)
                {
                    writer.Write("&");
                }
            }

            writer.Flush();
        }


        public static string SerializeQueryString(object parameters)
        {
            StringBuilder querystring = new StringBuilder();
            int i = 0;
            try
            {
                PropertyInfo[] properties;
                #if NETFX_CORE
                properties = parameters.GetType().GetTypeInfo().DeclaredProperties.ToArray();
                #else
                properties = parameters.GetType().GetProperties();
                #endif

                foreach (var property in properties)
                {
                    querystring.Append(property.Name + "=" + System.Uri.EscapeDataString(property.GetValue(parameters, null).ToString()));

                    if (++i < properties.Length)
                    {
                        querystring.Append("&");
                    }
                }
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentNullException("Paramters cannot be a null object",e);
            }
          
            return querystring.ToString();
        }
    }
}
