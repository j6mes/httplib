using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Collector
{
    internal abstract class BaseCollector
    {
        public static bool CollectStats = true;
        
        public static string baseUrl = "http://stats.httplib.com/api/stats/collect";

        protected abstract String CollectUrl {get;}

        public void Collect(Collection collection)
        {
            collection.platformid = "";
            collection.libversion = "";
#if NETFX_CORE || SILVERLIGHT
#else
            collection.platformid = Assembly.GetAssembly(typeof(BaseCollector)).GetName().Version.ToString();
            collection.libversion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            collection.appname = Assembly.GetEntryAssembly().GetName().Name;
            
#endif
            if (CollectStats && typeof(Install) == this.GetType()) 
            {
                Http.Post(CollectUrl).Form(collection).OnSuccess(s => { }).OnFail(e => { }).Go();
            }
            else if (CollectStats && !(collection.slug as string).Contains("stats.httplib.com"))
            {
                Http.Post(CollectUrl).Form(collection).OnSuccess(s => { }).OnFail(e=>{}).Go();
            }
        }
    }
}
