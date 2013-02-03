using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Noodles.Example.Models;
using Walkies;

namespace Noodles.Example.Domain
{
    [Show]
    public class Tasks : IEnumerable<Task>, IGetChild
    {
        private readonly List<Task> _tasks = new List<Task>();

        public Tasks()
        {
            this._tasks = new List<Task>();
        }

        public Task AddTask(string note)
        {
            var item = new Task() { Text = note }.SetParent(this, t => t.Name);
            _tasks.Add(item);
            return item;
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Name { get { return "tasks"; } }

        public void RemoveComplete()
        {
            this._tasks.RemoveAll(t => t.Completed);
        }

        public object this[string fragment]
        {
            get { return _tasks.SingleOrDefault(t => t.Name == fragment); }
        }
    }
}