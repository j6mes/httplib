using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JumpKick.HttpLib.Streams
{
    public delegate void ProgressChanged(long bytesRead, long? totalBytes);
    public delegate void Completed(long totalBytes);
    public class ProgressCallbackHelper
    {
        private long? totalBytes;
        private Stream from;
        private Stream to;
        public event ProgressChanged ProgressChanged = delegate{};
        public event Completed Completed = delegate{};
        internal void OnProgressChange(long length, long? totalBytes)
        {
            ProgressChanged(length, totalBytes);
        }

        internal void OnCompleted(long totalBytes)
        {
            Completed(totalBytes);
        }

        public ProgressCallbackHelper(Stream from, Stream to, long? totalBytes)
        {
            this.to = to;
            this.from = from;
            this.totalBytes = totalBytes;
        }


        public void Go()
        {
            int count = 0;
            byte[] buffer = new byte[4096];
            long length = 0;

            DateTime lastChangeNotification = DateTime.MinValue;

            if (totalBytes == null && from.CanSeek)
            {
                totalBytes = (long)from.Length;
            }

            while ((count = from.Read(buffer, 0, 4096)) != 0)
            {
                length += count;
                to.Write(buffer, 0, count);

                if (DateTime.Now.AddSeconds(-1) > lastChangeNotification)
                {
                    lastChangeNotification = DateTime.Now;
                    this.OnProgressChange(length, totalBytes);
                }


            }

            this.OnCompleted(length);
        }
    }
}
