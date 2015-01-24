using JumpKick.HttpLib.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Builder
{
    public partial class RequestBuilder
    {

        #region Auth
        private AuthenticationProvider authProvider;
        public RequestBuilder Auth(string username, string password)
        {
            authProvider = new BasicAuthenticationProvider(username, password);
            return this;
        }

        public RequestBuilder Auth(string text)
        {
            authProvider = new TextAuthenticationProvider(text);
            return this;
        }

        public RequestBuilder Auth(AuthenticationProvider provider)
        {
            authProvider = provider;
            return this;
        }
        #endregion

    }
}
