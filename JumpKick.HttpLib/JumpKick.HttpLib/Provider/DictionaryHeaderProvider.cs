using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public DictionaryHeaderProvider(object parameters)
        {
            headerData = new Dictionary<String, String>();
        
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
                    headerData.Add(property.Name, System.Uri.EscapeDataString(property.GetValue(parameters, null).ToString()));

                  
                }
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentNullException("Paramters cannot be a null object", e);
            }

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
