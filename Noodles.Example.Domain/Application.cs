using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [DisplayName("Your to do lists")]
    public class Application
    {
        public string Test { get { return "hello"; } }
        [Show(UiHint = "Noodles/LinkList")] 
        public List<ToDoList> Lists { get; private set; }

        public Application()
        {
            Lists = new List<ToDoList>();
            var toDoList = new ToDoList()
            {
                ListName = "Shopping List"
            };
            AddList(toDoList);
            toDoList.AddTask(new Task() {Title = "Milk"});
        }

        [Show]
        public void AddList(ToDoList list)
        {
            Lists.Add(list.SetParent(Lists, Guid.NewGuid().ToString()));
        }
         
    }
}