using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
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
            Membership = new Membership();
        }

        [Child]
        public Membership Membership { get; set; }

        [Child]
        public Settings Settings { get; set; }

       

    }

    public class User
    {
        [Show][Required]
        public string DisplayName { get; set; }
        [Show][Required][DataType(DataType.Password)]
        public string Password { get; set; }
        [Show][Required][DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class MembershipSettings
    {
        [Show]
        public bool AllowAnonymousAccess { get; set; }
        [Show]
        public bool AllowSelfRegistration { get; set; }
    }
}