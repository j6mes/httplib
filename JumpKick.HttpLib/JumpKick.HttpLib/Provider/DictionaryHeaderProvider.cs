using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Provider
{
    public class DictionaryHeaderProvider : HeaderProvider
    {
        private IDictionary<String, String> headerData;

        public DictionaryHeaderProvider(IDictionary<String, String> data)
        {
            headerData = data;
        }

        public Header[] GetHeaders()
        {
            List<Header> headers = new List<Header>();

            foreach (KeyValuePair<String, String> kvp in headerData)
            {
                headers.Add(new Header(kvp.Key, kvp.Value));
            }

            return headers.ToArray();
        }
    }
}
