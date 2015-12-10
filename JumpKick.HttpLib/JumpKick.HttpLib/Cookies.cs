using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib
{
    public class Cookies
    {
        protected static CookieContainer cookies = new CookieContainer();

        public static void ClearCookies()
        {
            cookies = new CookieContainer();
        }

        public static void SetCookies(CookieContainer cookieContainer)
        {
            cookies = cookieContainer;
        }

        public static CookieContainer Container { get { return cookies; } }
    }
}
