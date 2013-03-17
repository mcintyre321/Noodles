using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noodles.AspMvc.UiAttributes;
namespace Noodles.Example.Domain.Tasks
{
    public class ToDoLists
    {
        public ToDoLists()
        {
            Lists = new List<ToDoList>();
            var toDoList = new ToDoList()
            {
                ListName = "Shopping List"
            };

            AddList(toDoList);
            toDoList.AddTask(new Task() { Title = "Milk" });
        }
        public ICollection<ToDoList> Lists { [ShowAsLinkList] get; private set; }

        [Show]
        public void AddList(ToDoList list)
        {
            Lists.Add(list);
        }
    }
}
