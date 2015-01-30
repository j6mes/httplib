using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Provider
{
    public interface ActionProvider
    {
        Action<WebHeaderCollection, Stream> Success {get;}
        Action<WebException> Fail { get; }
    }
}
