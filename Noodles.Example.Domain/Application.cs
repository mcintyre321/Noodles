using System;
using System.Collections;
using System.Collections.Generic;
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
            NotShownMessage = "This will not be shown as it is not marked with [Show]";

        }

        public string WelcomeMessage { [Show] get; set; }
        public string NotShownMessage { get; set; }

        [Link(UiHint = "TopBar.LeftItems")]
        public Membership Membership { get; set; }

        [Link(UiHint = "TopBar.RightItems")]
        public Settings Settings { get; set; }

        [Show(UiHint = "TopBar.RightItems")][HttpGet]
        public RedirectResult API()
        {
            return new RedirectResult("/api");
        }
        
        [Show]
        public IEnumerable<OrganisationSummary> YourOrganisations
        {
            get
            {
                yield return new OrganisationSummary(){Name = "Your Projects"};
                yield return new OrganisationSummary(){Name = "Acmecorps Projects"};

            }
        }
    }

    public class OrganisationSummary
    {

        public string Name {[Show] get; set; }
        [Show]
        public IEnumerable<ProjectSummary> YourProjects
        {
            get
            {
                yield return new ProjectSummary() { Name = "Project A" };
                yield return new ProjectSummary() { Name = "Project B" };
            }
        }
    }

    public class ProjectSummary
    {
        public string Name
        {
            [Show]
            get;
            set;
        }
    }
}