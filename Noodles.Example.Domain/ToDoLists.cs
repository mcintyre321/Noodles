using System.Collections;
using System.Collections.Generic;
using Noodles.Example.Models;
using Walkies;

namespace Noodles.Example.Domain
{
    public class ToDoLists : IEnumerable<ToDoList>
    {

        [Show]
        public void AddList([MyStringLength(1, 20)] string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) throw new UserException("Task description cannot be empty");
            var item = new ToDoList() { Description = listName }.SetParent(this, l => l.UniqueId);
            _items.Add(item);
        }

        List<ToDoList> _items = new List<ToDoList>();
 
        public IEnumerator<ToDoList> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}