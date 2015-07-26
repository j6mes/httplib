using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib
{
    public class HttpLib 
    {
        protected static CookieContainer cookies = new CookieContainer();

        public static void ClearCookies()
        {
            cookies = new CookieContainer();
        }
    }
}
