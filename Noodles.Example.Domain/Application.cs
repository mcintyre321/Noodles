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
        [Children]
        private IList<ToDoList> _lists = new List<ToDoList>();

        public Application()
        {
            var toDoList = new ToDoList()
            {
                Description = "Shopping List"
            };
            toDoList.AddTask("Milk");
            toDoList.AddTask("Bread");
            _lists.Add(toDoList);
        }

        [Show]
        public void AddList([MyStringLength(1, 20)] string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) throw new UserException("Task description cannot be empty");
            var item = new ToDoList() {Description = listName};
            _lists.Add(item);
        }
         
    }
}