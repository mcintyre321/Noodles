using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using Noodles.Attributes;
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


        [Show]
        public Membership Membership { get; private set; }

        [Show]
        public Settings Settings { get; private set; }

        [Show]
        [HttpGet][DisplayName("API")]
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

        [Show][TransformFf]
        public string SomeSimpleThing { get { return "Hello there"; } }

        [Children("Name")]
        public IList<Organisation> Items { get; private set; }
    }

    [DisplayName("{Name}")]
    public class Organisation
    {
        public Organisation(string name)
        {
            Name = name;
            About = "About the organisation";
            Projects = new List<Project>();
            Settings = new OrganisationSettings();
        }


        public string Name { [Show] get; set; }
        public string About { [Show] get; set; }
        
        [Show]
        public void SetDetails([Default("{Name}")][Required] string name, [Default("{About}"), Required, StringLength(100)] string about)
        {
            Name = name;
            About = about;
        }

        [Children("Name")]
        public List<Project> Projects { get; set; }

        [Show][Description("This description was added using a [Description] attribute")]
        public void AddNewProject([Required][StringLength(50, MinimumLength = 5)] String name)
        {
            Projects.Add(new Project(name));
        }

        [Show]
        public OrganisationSettings Settings { get; private set; }

        [Show]
        public void MethodWithTupleChoices(string someValue)
        {
        }
        public IEnumerable<Tuple<string, string>> MethodWithTupleChoices_someValue_choices()
        {
            yield return Tuple.Create("Some option", "option1");
            yield return Tuple.Create("Some other option", "option2");
        }
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
        [Show]
        public ToDoLists ToDoLists { get; private set; }

        [Show ]
        public Discussions.DiscussionsManager DiscussionsManager { get; private set; }
        
       
        [Show]
        [DisplayName][Required][StringLength(50, MinimumLength = 5)]
        public string Name
        {
            get; set;
        }
    }
}