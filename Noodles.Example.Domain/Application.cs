using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Noodles.Example.Domain.Tasks;
using Walkies;

namespace Noodles.Example.Domain
{
    [DisplayName("Project Organiser")]
    public class Application
    {
        [Child]
        public ToDoLists ToDoLists { get; private set; }
        
        [Child]
        public Discussions.DiscussionsManager DiscussionsManager { get; private set; }

        public Application()
        {
            ToDoLists = new ToDoLists();
            DiscussionsManager = new Discussions.DiscussionsManager();
            Settings = new Settings();
        }

       
        [Child]
        public Settings Settings { get; set; }
         
    }
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

    public class ProfileSettings
    {
        [Show]
        public bool ShowEmailAddressesToMembers { get; set; }
        [Show]
        public bool EnableWall { get; set; }
    }

    public class MembershipSettings
    {
        [Show]
        public bool AllowAnonymousAccess { get; set; }
        [Show]
        public bool AllowSelfRegistration { get; set; }
    }
}