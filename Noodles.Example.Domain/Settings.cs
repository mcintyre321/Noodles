
namespace Noodles.Example.Domain
{
    [UiHint("SideMenu")]
    public class Settings
    {
        public Settings()
        {
            ProfileSettings = new ProfileSettings();
            MembershipSettings = new MembershipSettings();
        }

        [Link]
        public ProfileSettings ProfileSettings { get; private set; }
        [Link]
        public MembershipSettings MembershipSettings { get; private set; }
    }
}