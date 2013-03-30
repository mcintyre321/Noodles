using System;
using System.Text;
using System.Threading.Tasks;
using Noodles.Models;

namespace Noodles.WebApi.Models
{
    public class LinkVm
    {
        public string Relationship { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
        public string MediaType { get; set; }

        public LinkVm(NodeLink obj, string relationship)
        {
            Relationship = relationship;
            Name = obj.DisplayName;
            MediaType = obj.TargetType.FullName;
            this.Url = obj.Url;
        }
    }
}
