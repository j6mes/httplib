using JumpKick.HttpLib.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Builder
{
    public partial class RequestBuilder
    {

        #region Headers
        private HeaderProvider headerProvider;

        public RequestBuilder Headers(object header)
        {
            Dictionary<String, String> headers = new Dictionary<String, String>();
            PropertyInfo[] properties;
#if NETFX_CORE
            properties = header.GetType().GetTypeInfo().DeclaredProperties.ToArray();
#else
            properties = header.GetType().GetProperties();
#endif

            foreach (var property in properties)
            {
                headers.Add(property.Name, System.Uri.EscapeDataString(property.GetValue(header, null).ToString()));
            }
            headerProvider = new DictionaryHeaderProvider(headers);
            return this;
        }

        public RequestBuilder Headers(IDictionary<String, String> header)
        {
            this.headerProvider = new DictionaryHeaderProvider(header);
            return this;
        }

        public RequestBuilder Headers(HeaderProvider headerProvider)
        {
            this.headerProvider = headerProvider;
            return this;
        }

        #endregion

    }
}
