using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Provider
{
    public class RawBodyProvider : BodyProvider
    {

        public string GetContentType()
        {
            throw new NotImplementedException();
        }

        public Stream GetBody()
        {
            throw new NotImplementedException();
        }
    }
}
