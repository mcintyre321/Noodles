namespace Noodles.AspMvc.Models.Layout
{
    public class LayoutVm
    {
        public LayoutVm()
        {
            TopBar = new TopBarVm();
            Footer = new FooterVm();
            HeadBundle = new BundleVm();
            BodyBottomBundle = new BundleVm();
        }

        public string Title { get; set; }

        public TopBarVm TopBar { get; set; }
        public FooterVm Footer { get; set; }

        public BundleVm HeadBundle { get; set; }
        public BundleVm BodyBottomBundle { get; set; }

        public bool IsFluid { get; set; }
    }
}