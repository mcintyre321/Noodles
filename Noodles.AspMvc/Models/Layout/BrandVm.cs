namespace Noodles.AspMvc.Models.Layout
{
    public class BrandVm
    {
        public BrandVm()
        {
            Html = "Application";
        }
        public string Html { get; set; }
        public string Href { get; set; }
    }
}