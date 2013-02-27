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

    public class Settings
    {
    }
}