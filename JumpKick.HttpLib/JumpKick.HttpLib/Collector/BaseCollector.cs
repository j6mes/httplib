using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Collector
{
    public abstract class BaseCollector
    {
        public static bool CollectStats = true;

        protected abstract static string id;
        protected static string baseUrl = "http://stats.httplib.com/api/statis/collect";

        protected abstract String CollectUrl {get;}

        public static void Collect(Collection collection)
        {
            if (CollectStats)
            {
                Http.Post(CollectUrl).Body(id).Go();
            }
        }
    }
}
