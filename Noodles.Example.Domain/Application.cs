using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Noodles.Example.Domain.Tasks;

namespace Noodles.Example.Domain
{
    [DisplayName("Project Organiser")]
    public class Application
    {
        [Link]
        public ToDoLists ToDoLists { get; private set; }
        
        [Link]
        public Discussions.DiscussionsManager DiscussionsManager { get; private set; }

        public Application()
        {
            ToDoLists = new ToDoLists();
            DiscussionsManager = new Discussions.DiscussionsManager();
            Settings = new Settings();
            Membership = new Membership();
        }

        [Link]
        public Membership Membership { get; set; }

        [Link]
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