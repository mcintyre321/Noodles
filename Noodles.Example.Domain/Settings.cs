
namespace Noodles.Example.Domain
{
    public class Settings
    {
        public Settings()
        {
            ProfileSettings = new ProfileSettings();
            MembershipSettings = new MembershipSettings();
        }

        [Show]
        public ProfileSettings ProfileSettings { get; private set; }
        [Show]
        public MembershipSettings MembershipSettings { get; private set; }
    }
}