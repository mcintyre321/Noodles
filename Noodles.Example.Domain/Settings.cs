using Walkies;

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

        [Child]
        public ProfileSettings ProfileSettings { get; private set; }
        [Child]
        public MembershipSettings MembershipSettings { get; private set; }
    }
}