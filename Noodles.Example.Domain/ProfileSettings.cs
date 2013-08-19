namespace Noodles.Example.Domain
{
    public class ProfileSettings
    {
        public ProfileSettings()
        {
            ShowEmailAddressesToMembers = true;
        }
        [Show]
        public bool ShowEmailAddressesToMembers { get; set; }
        [Show]
        public bool EnableWall { get; set; }

    }
}