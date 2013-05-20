using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Noodles.Example.Domain.Tasks;

namespace Noodles.Example.Domain
{
    [DisplayName("Project Organiser")]
    public class Application
    {
        [Behaviour][JsonIgnore]
        private AuthBehaviour AuthBehaviour = new AuthBehaviour();

        [Link(UiHint = "TopBar.LeftItems")]
        public ToDoLists ToDoLists { get; private set; }

        [Link(UiHint = "TopBar.LeftItems")]
        public Discussions.DiscussionsManager DiscussionsManager { get; private set; }

        public Application()
        {
            ToDoLists = new ToDoLists();
            DiscussionsManager = new Discussions.DiscussionsManager();
            Settings = new Settings();
            Membership = new Membership()
            {
                Users =
                {
                    new User()
                    {
                        DisplayName= "Mr Example",
                        Email = "example@email.com",
                        Password = "password"
                    }
                }
            };
            WelcomeMessage = "See https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Domain/Application.cs";
        }

        public string WelcomeMessage { [Show] get; set; }

        [Link(UiHint = "TopBar.LeftItems")]
        public Membership Membership { get; set; }

        [Link(UiHint = "TopBar.RightItems")]
        public Settings Settings { get; set; }

        [Show(UiHint = "TopBar.RightItems")][HttpGet]
        public RedirectResult API()
        {
            return new RedirectResult("/api");
        }
    }

}