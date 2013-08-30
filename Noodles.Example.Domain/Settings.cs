
namespace Noodles.Example.Domain
{
    public class Settings
    {
        public Settings()
        {
            ProfileSettings = new ProfileSettings();
            MembershipSettings = new MembershipSettings();
        }

        [Show (UiHint = "SideNav")]
        public ProfileSettings ProfileSettings { get; private set; }
        [Show(UiHint = "SideNav")]
        public MembershipSettings MembershipSettings { get; private set; }
    }
}