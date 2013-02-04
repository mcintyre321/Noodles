using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        [Show]
        public void AddTask([MyStringLength(1, 20)] string taskDescription)
        {
            if (string.IsNullOrWhiteSpace(taskDescription)) throw new UserException("Task description cannot be empty");
            var item = new Task() { Text = taskDescription }.SetParent(this, t => t.Name);
            _tasks.Add(item);
        }

        [Show]
        public void ClearCompletedTasks()
        {
            this._tasks.RemoveAll(t => t.Completed);
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

        public object this[string fragment]
        {
            get { return _tasks.SingleOrDefault(t => t.Name == fragment); }
        }

       
    }
}