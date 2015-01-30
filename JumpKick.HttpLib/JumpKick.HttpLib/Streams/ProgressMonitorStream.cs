using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace JumpKick.HttpLib.Streams
{
    

    public static class ProgressMonitorStream
    {

        public static ProgressCallbackHelper CopyToProgress(this Stream from, Stream to, long? totalBytes)
        {
           
            ProgressCallbackHelper callback = new ProgressCallbackHelper(from,to,totalBytes);



            

            return callback;

       
        }
    }
}
