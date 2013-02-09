using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [Name("{Description}")]
    public class ToDoList
    {
        [Children]
        private readonly List<Task> _tasks = new List<Task>();
        public ToDoList()
        {
            this._tasks = new List<Task>();
        }

        [Show]
        public void AddTask(Task task)
        {
            _tasks.Add(task);
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
    }
}