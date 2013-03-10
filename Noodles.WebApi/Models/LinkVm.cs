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
        public string Url { get; set; }
        public string MediaType { get; set; }

        public LinkVm(INode obj, string relationship)
        {
            Relationship = relationship;
            Name = obj.DisplayName;
            MediaType = obj.ValueType.FullName;
            this.Url = obj.Url;
        }
    }
}
