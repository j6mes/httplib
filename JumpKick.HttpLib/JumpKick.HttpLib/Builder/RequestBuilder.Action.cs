﻿using JumpKick.HttpLib.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JumpKick.HttpLib.Streams;
namespace JumpKick.HttpLib.Builder
{
    public partial class RequestBuilder
    {

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
            this.success = action;
            return this;
        }

        public RequestBuilder DownloadTo(String filePath)
        {
#if NETFX_CORE
            test
#else


            this.success = (headers, result) =>
            {

                long? length = null;
                if (headers.AllKeys.Contains("Content-Length")) { length = long.Parse(headers["Content-Length"]); }
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                ProgressCallbackHelper operation = result.CopyToProgress(fs,length);
                operation.Completed += (totalbytes)=>{fs.Close();};
                
            };
            return this;
#endif
        }



        public RequestBuilder DownloadTo(String filePath, Action<long,long?> OnProgressChanged)
        {
#if NETFX_CORE
            test
#else


            this.success = (headers, result) =>
            {

                long? length = null;
                if (headers.AllKeys.Contains("Content-Length")) { length = long.Parse(headers["Content-Length"]); }
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                ProgressCallbackHelper operation = result.CopyToProgress(fs, length);

                operation.ProgressChanged += (copied, total) => { OnProgressChanged(copied, total); };
                operation.Completed += (totalbytes) => { fs.Close(); };

            };
            return this;
#endif
        }




        public RequestBuilder DownloadTo(String filePath, Action<WebHeaderCollection> onSuccess)
        {
#if NETFX_CORE
            test
#else


            this.success = (headers, result) =>
            {
                long? length = null;
                if (headers.AllKeys.Contains("Content-Length")) { length = long.Parse(headers["Content-Length"]); }

                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);

                ProgressCallbackHelper operation = result.CopyToProgress(fs, length);

                operation.Completed += (totalbytes) => { fs.Close(); onSuccess(headers); };

               
            };
            return this;
#endif
        }

        public RequestBuilder DownloadTo(String filePath, Action<long, long?> onProgressChanged,Action<WebHeaderCollection> onSuccess)
        {
#if NETFX_CORE
            test
#else


            this.success = (headers, result) =>
            {
                long? length = null;
                if(headers.AllKeys.Contains("Content-Length")) {length = long.Parse( headers["Content-Length"]);}
                
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                ProgressCallbackHelper operation = result.CopyToProgress(fs, length);

                operation.ProgressChanged += (copied, total) => { onProgressChanged(copied, total); };
                 operation.Completed += (totalbytes) => { fs.Close(); onSuccess(headers); };
                 operation.Go();
            };
            return this;
#endif
        }


        public RequestBuilder AppendTo(String filePath)
        {
            this.success = (headers, result) =>
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                result.CopyTo(fs);
                fs.Close();
            };
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

        public ActionProvider GetAction()
        {
            return actionProvider;
        }

        #endregion
  
    }
}
