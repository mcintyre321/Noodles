using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodles.WebApi.Models
{
    public class NodeVm
    {
        public Uri Url { get; set; }

        public string NodeType
        {
            get { return this.GetType().FullName.Replace("Vm", ""); }
        }
    }
}
