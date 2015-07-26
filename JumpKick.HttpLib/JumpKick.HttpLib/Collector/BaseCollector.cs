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

        protected abstract string id;
        protected string collectUrl;

        protected void Collect()
        {
            if (CollectStats)
            {
                Http.Post(collectUrl).Body(id).Go();
            }
        }
    }
}
