using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FormFactory.Attributes;
using Newtonsoft.Json;
using Noodles.AspMvc.UiAttributes;
using Noodles.Example.Domain.Tasks;

namespace Noodles.Example.Domain
{
    [DisplayName("Project Organiser")]
    public class Application
    {

        [Behaviour]
        [JsonIgnore]
        private AuthBehaviour AuthBehaviour = new AuthBehaviour();



        public Application()
        {

            Settings = new Settings();
            Membership = new Membership();
           
            Organisations = new Organisations();
            
        }

        [Link(UiHint = "Inline")]
        public Organisations Organisations { get; set; }


        [Link(UiHint = "TopBar.LeftItems")]
        public Membership Membership { get; set; }

        [Link(UiHint = "TopBar.RightItems")]
        public Settings Settings { get; set; }

        [Show(UiHint = "TopBar.RightItems")]
        [HttpGet]
        public RedirectResult API()
        {
            return new RedirectResult("/api");
        }


    }

    public class Organisations
    {
        public Organisations()
        {
            Items = new List<Organisation>();
        }

        [ShowAsTable, NoLabel]
        public List<Organisation> Items { get; set; }
    }


    public class Organisation
    {
        public Organisation(string name)
        {
            Name = name;
            Projects = new List<Project>();
        }
        [Show]
        public string Name { get; private set; }

        [ShowAsTable]
        public List<Project> Projects { get; set; }
    }

    public class Project
    {
        public Project()
        {
            ToDoLists = new ToDoLists();
            DiscussionsManager = new Discussions.DiscussionsManager();
        }
        public ToDoLists ToDoLists { get; private set; }

        public Discussions.DiscussionsManager DiscussionsManager { get; private set; }
        public Project(string name)
        {
            Name = name;
        }

        public string Name
        {
            [Show]
            get;
            private set;
        }
    }
}