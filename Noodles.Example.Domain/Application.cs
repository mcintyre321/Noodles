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
using Harden.ValidationAttributes;
using Newtonsoft.Json;
using Noodles.AspMvc.RequestHandling.Transforms;
using Noodles.AspMvc.UiAttributes;
using Noodles.AspMvc.UiAttributes.Icons;
using Noodles.Example.Domain.Tasks;
using Noodles.Models;

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

        [Show]
        public Organisations Organisations { get; private set; }


        [Show(UiHint = "TopBar.LeftItems")]
        public Membership Membership { get; private set; }

        [Show(UiHint = "TopBar.RightItems")]
        public Settings Settings { get; private set; }

        [Show(UiHint = "TopBar.RightItems")]
        [HttpGet][Icon(IconNames.cog)][DisplayName("API")]
        public RedirectResult API()
        {
            return new RedirectResult("/api");
        }


    }

    [DisplayName("Organisations")]
    public class Organisations
    {
        public Organisations()
        {
            Items = new List<Organisation>();
        }

        [Show]
        public List<Organisation> Items { get; private set; }
    }

    [DisplayName("{Name}")]
    public class Organisation
    {
        public Organisation(string name)
        {
            Name = name;
            Projects = new List<Project>();
            Settings = new OrganisationSettings();
        }
        [Show] 
        public string Name { get; private set; }

        public List<Project> Projects { [Show] get; set; }

        [Show]
        public void AddNewProject([Required][StringLength(50, MinimumLength = 5)] String name)
        {
            Projects.Add(new Project(name));
        }

        public OrganisationSettings Settings { [Show] get; set; }
    }

    public class OrganisationSettings
    {
        public OrganisationSettings()
        {
            RegistrationMode = new Public();
        }
        [Show]
        public RegistrationMode RegistrationMode { get; set; }
        public IEnumerable<RegistrationMode> RegistrationMode_choices()
        {
            yield return new Public();
            yield return new InviteOnly();
        } 
    }

    public class InviteOnly : RegistrationMode
    {
    }

    public class Public : RegistrationMode
    {
    }

    public abstract class RegistrationMode
    {
    }

    public class Project
    {
        public Project(string name) 
        {
            Name = name;
            ToDoLists = new ToDoLists();
            DiscussionsManager = new Discussions.DiscussionsManager();
        }
        [Show(UiHint = "Inline")]
        [NotInTable]
        public ToDoLists ToDoLists { get; private set; }

        [Show(UiHint = "Inline")]
        [NotInTable]
        public Discussions.DiscussionsManager DiscussionsManager { get; private set; }
        
       
        [Show]
        [NotInTable]
        [DisplayName]
        public string Name
        {
            get; set;
        }
    }
}