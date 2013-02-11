using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [Name("Your to do lists")]
    public class Application
    {
        [Show]
        public string Title { get; set; }
        [Show]
        public string Title2 { get { return "asd"; } }

        [Show] public IList<ToDoList> Lists { get; private set; }

        public Application()
        {
            Lists = new List<ToDoList>();
            var toDoList = new ToDoList()
            {
                Description = "Shopping List"
            };
            Lists.Add(toDoList);
        }

        [Show]
        public void AddList([MyStringLength(1, 20)] string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) throw new UserException("Task description cannot be empty");
            var item = new ToDoList() {Description = listName};
            Lists.Add(item);
        }
         
    }
}