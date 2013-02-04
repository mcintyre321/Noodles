using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [Name("{Description}")]
    public class ToDoList
    {
        public ToDoList()
        {
            this.UniqueId = Guid.NewGuid().ToString();
            this.Tasks = new Tasks();
            Tasks.AddTask("This is an example task");
            this.Description = "An example todo list";
        }

        [Show]
        public string Description
        {
            get;
            [DataType(DataType.MultilineText)]
            set;
        }

        [Child]
        public Tasks Tasks { get; private set; }

        public string UniqueId { get; private set; }
    }
}