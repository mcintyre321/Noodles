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
        }
        public ICollection<ToDoList> Lists { [ShowAsLinkList] get; private set; }

        [Show]
        public void AddList(ToDoList list)
        {
            Lists.Add(list);
        }
    }
}
