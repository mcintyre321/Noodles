using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [Name("{Description}")]
    public class ToDoList : IHasChildren
    {
        private readonly List<Task> _tasks = new List<Task>();

        public ToDoList()
        {
            this._tasks = new List<Task>();
            this.UniqueId = Guid.NewGuid().ToString();
        }

        [Show]
        public void AddTask([MyStringLength(1, 20)] string taskDescription)
        {
            if (string.IsNullOrWhiteSpace(taskDescription)) throw new UserException("Task description cannot be empty");
            _tasks.Add(new Task() { Text = taskDescription });
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

        [Show]
        public string Description { get; set; }

        public string UniqueId { get; private set; }
         
        public IEnumerable<Tuple<string, object>> Children { get { return _tasks.Select(i => Tuple.Create(i.UniqueId, (object)i)); } }
    }
}