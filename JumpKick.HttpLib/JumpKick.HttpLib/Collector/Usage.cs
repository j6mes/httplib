using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpKick.HttpLib.Collector
{
    internal class Usage : BaseCollector
    {

        protected override string CollectUrl
        {
            get { return BaseCollector.baseUrl + "/use"; }
        }
    }
}
