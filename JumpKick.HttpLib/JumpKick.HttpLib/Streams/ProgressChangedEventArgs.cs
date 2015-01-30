using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Streams
{
    public struct ProgressChangedEventArgs 
    {
        public long BytesCopied { get; private set; }
        public long? TotalBytes { get; private set; }
        
    }
}
