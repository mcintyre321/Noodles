using System;
using System.Collections.Generic;

namespace Noodles.Example.Domain
{
    public class MembershipSettings
    {
        [Show]
        public bool AllowAnonymousAccess { get; set; }
        [Show]
        public bool AllowSelfRegistration { get; set; }

        [Show]
        public string InvitationMode { get; set; }
        public IEnumerable<string> InvitationMode_choices()
        {
            return "Anyone can invite, Only admins can invite".Split(',');
        }

        [Show]//this is here to test that nested method panels are the right width
        public void ResetAllPasswords()
        {
            
        }
    }
}