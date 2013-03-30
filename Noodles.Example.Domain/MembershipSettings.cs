namespace Noodles.Example.Domain
{
    public class MembershipSettings
    {
        [Show]
        public bool AllowAnonymousAccess { get; set; }
        [Show]
        public bool AllowSelfRegistration { get; set; }

        [Show]//this is here to test that nested method panels are the right width
        public void ResetAllPasswords()
        {
            
        }
    }
}