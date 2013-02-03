namespace Noodles.Example.WebApi.Models
{
    public class LinkVm
    {
        public string Relationship { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public LinkVm(object obj, string relationship, string name)
        {
            Relationship = relationship;
            Name = name;
            this.Url = obj.Url();
        }
    }
}