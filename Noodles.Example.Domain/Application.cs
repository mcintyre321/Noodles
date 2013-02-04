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
        private List<ToDoList> _items = new List<ToDoList>();

        public Application()
        {
            var toDoList = new ToDoList()
            {
                Description = "Shopping List"
            };
            toDoList.AddTask("Milk");
            toDoList.AddTask("Bread");
            _items.Add(toDoList);
        }

        [Show]
        public void AddList([MyStringLength(1, 20)] string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) throw new UserException("Task description cannot be empty");
            var item = new ToDoList() {Description = listName};
            _items.Add(item);
        }
         
    }
}