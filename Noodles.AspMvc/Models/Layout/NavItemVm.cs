namespace Noodles.AspMvc.Models.Layout
{
    public class NavItemVm
    {
        public NavItemVm()
        {
            Html = "";
        }
        public string Html { get; set; }
        public bool Active { get; set; }
    }
}