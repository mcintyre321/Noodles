using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Noodles.AspMvc.RequestHandling.Transforms;
using Noodles.AspMvc.UiAttributes;

namespace Noodles.Example.Domain
{
    public class MembershipSettings
    {
        public MembershipSettings()
        {
            Advanced = new AdvancedMembershipSettings();
        }
        [Show]
        public bool AllowAnonymousAccess { get; set; }
        [Show]
        public bool AllowSelfRegistration { get; set; }

        [Show][Description("Choices loaded via ajax")]
        public string Country { get; set; }
        
        [Show][RemoveFromView]
        public string[] Country_QueryChoices(string query)
        {
            var results = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new RegionInfo(c.LCID))
                .Select(ri => ri.DisplayName).Distinct().Where(name => name.StartsWith(query ?? Guid.NewGuid().ToString()));
            return results.ToArray();
        }

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

        [Show]
        public AdvancedMembershipSettings Advanced { get; private set; }
    }

   

    public class AdvancedMembershipSettings
    {
        [Show]
        public bool SomeAdvancedSetting { get; set; }
    }
}