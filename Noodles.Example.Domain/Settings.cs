
namespace Noodles.Example.Domain
{
    public class Settings
    {
        public Settings()
        {
            ProfileSettings = new ProfileSettings();
            MembershipSettings = new MembershipSettings();
        }

        [Link (UiHint = "SideNav")]
        public ProfileSettings ProfileSettings { get; private set; }
        [Link(UiHint = "SideNav")]
        public MembershipSettings MembershipSettings { get; private set; }
    }
}