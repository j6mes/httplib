using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Collector
{
    internal class Install : BaseCollector
    {
        protected override string CollectUrl
        {
            get { return BaseCollector.baseUrl + "/install"; }
        }

        internal Install()
        {
            this.Collect(new Collection{ });
        }
    }
}
