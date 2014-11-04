using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Provider
{
    public class MultiHeaderProvider : HeaderProvider
    {
        protected IEnumerable<HeaderProvider> providers;

        public MultiHeaderProvider(IEnumerable<HeaderProvider> providers)
        {
            this.providers = providers;
        }

        public MultiHeaderProvider(params HeaderProvider[] providers)
        {
            this.providers = providers.AsEnumerable();
        }


        public Header[] GetHeaders()
        {
            List<Header> headers = new List<Header>();
            foreach(HeaderProvider provider in providers) 
            {
                headers.AddRange(provider.GetHeaders());
            }
            return headers.ToArray();
        }
    }
}
