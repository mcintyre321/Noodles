using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [Name("Your to do lists")]
    public class Application : IEnumerable<ToDoList>, IHasChildren
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

        public object this[string fragment]
        {
            get { return this.SingleOrDefault(i => i.UniqueId == fragment); }
        }


        public IEnumerable<Tuple<string, object>> Children
        {
            get { return _items.Select(i => Tuple.Create(i.GetName(), (object) i)); }
        }
    }
}