namespace Noodles.WebApi.Models
{
    public class LinkVm
    {
        public string Relationship { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public LinkVm(object obj, string relationship, string name)
        {
            Relationship = relationship;
            Name = obj.GetName();
            this.Url = obj.Url();
        }
    }
}