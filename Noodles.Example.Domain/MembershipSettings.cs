namespace Noodles.Example.Domain
{
    public class MembershipSettings
    {
        [Show]
        public bool AllowAnonymousAccess { get; set; }
        [Show]
        public bool AllowSelfRegistration { get; set; }
    }
}