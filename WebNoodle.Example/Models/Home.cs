using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebNoodle.Example.Models
{
    public class Home : IHasChildren
    {
        public Home()
        {
            this._tasks = new Tasks(this);
            this._tasks.AddTask("This is an example task");

        }

        private Tasks _tasks;

        public IEnumerable<Task> Tasks { get { return _tasks; } }

        public void AddNote(string note)
        {
            _tasks.AddTask(note);
        }

        public void ClearCompletedTasks()
        {
            _tasks.RemoveComplete();
        }

        public object GetChild(string name)
        {
            if (name.ToLowerInvariant() == _tasks.Name) return _tasks;
            return null;
        }
    }

    public class Tasks : IEnumerable<Task>, IHasChildren, IHasName, IHasParent<Home>
    {
        private readonly List<Task> _tasks = new List<Task>();

        public Tasks(Home parent)
        {
            Parent = parent;
            this._tasks = new List<Task>();
        }

        public void AddTask(string note)
        {
            _tasks.Add(new Task(this) { Text = note });
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return _tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object GetChild(string name)
        {
            return _tasks.SingleOrDefault(task => task.Name == name);
        }

        public Home Parent { get; private set; }

        public string Name { get { return "tasks"; } }

        public void RemoveComplete()
        {
            this._tasks.RemoveAll(t => t.Completed);
        }
    }

    

    public class Task : IHasName, IHasParent<Tasks>
    {
        public Task(Tasks parent)
        {
            Parent = parent;
            Name = Guid.NewGuid().ToString();
        }

        public string Text { get; set; }
        public bool Completed { get; set; }

        public string Name { get; private set; }

        public Tasks Parent { get; private set; }
    }
}