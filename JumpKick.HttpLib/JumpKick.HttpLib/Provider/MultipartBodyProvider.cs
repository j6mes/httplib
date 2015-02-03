
namespace JumpKick.HttpLib.Provider
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class MultipartBodyProvider : DefaultBodyProvider
    {
        private string boundary;
        

        private Stream contentstream;
        private StreamWriter writer;
        private IList<NamedFileStream> files;
        private object parameters;
        public Action<long, long?> OnProgressChangeCallback = (a, b) => { };
        public Action<long> OnCompletedCallback = (a) => { };


        public MultipartBodyProvider()
        {
            boundary = RandomString(8);
            contentstream = new MemoryStream();
            writer = new StreamWriter(contentstream);
            files = new List<NamedFileStream>();
        }

        public void AddFile(NamedFileStream file)
        {
            files.Add(file);
        }

        public override string GetContentType()
        {
            return string.Format("multipart/form-data, boundary={0}", boundary);
        }

        public string GetBoundary()
        {
            return boundary;
        }

        public void SetParameters(object parameters)
        {
            this.parameters = parameters;
        }


        public override Stream GetBody()
        {
            writer.Write("\n");

            /*
             * Serialize parameters in multipart manner
             */
            if (parameters != null)
            {

                IEnumerable<PropertyInfo> properties;
#if NETFX_CORE
                properties = parameters.GetType().GetTypeInfo().DeclaredProperties;
#else
                properties = parameters.GetType().GetProperties();
#endif
                using(var enumerator = properties.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var property = enumerator.Current;
                        writer.Write(string.Format("--{0}\ncontent-disposition: form-data; name=\"{1}\"\n\n{2}\n", boundary, System.Uri.EscapeDataString(property.Name), System.Uri.EscapeDataString(property.GetValue(parameters, null).ToString())));
                        writer.Flush();
                    }
                }
            }

            /*
             * A boundary string that we'll reuse to separate files
             */
            string closing = string.Format("\n--{0}--\n", boundary);


            /*
             * Write each files to the postStream
             */
            foreach (NamedFileStream file in files)
            {
                /*
                 * Additional info that is prepended to the file
                 */
                string separator = string.Format("--{0}\ncontent-disposition: form-data; name=\"{1}\"; filename=\"{2}\"\nContent-Type: {3}\n\n",boundary,file.Name,file.Filename,file.ContentType);
                writer.Write(separator);
                writer.Flush();



                /*
                 * Read the file into the output buffer
                 */
                
                StreamReader sr = new StreamReader(file.Stream);
           
                int bytesRead = 0;
                byte[] buffer = new byte[4096];

                while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    contentstream.Write(buffer, 0, bytesRead);
                }

#if NETFX_CORE
                file.Stream.Dispose();
#else
                file.Stream.Close();
#endif


                /*
                 * Write the delimiter to the output buffer
                 */
                writer.Write(closing, 0, closing.Length);
                writer.Flush();
            }
      

            contentstream.Seek(0, SeekOrigin.Begin);
            return contentstream;

        }


        /*
         * Muhammad Akhtar @StackOverflow
         */
        private static string RandomString(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[length];

            Random rd = new Random();
         
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }


        public override void OnProgressChange(long bytesSent, long? totalBytes)
        {
            OnProgressChangeCallback(bytesSent, totalBytes);
        }

        public override void OnCompleted(long totalBytes)
        {
            OnCompletedCallback(totalBytes);
        }
    }
}
