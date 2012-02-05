using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebNoodle.Example.Models
{
    public class ToDoList : IHasChildren
    {
        public ToDoList()
        {
            this._tasks = new Tasks(this);
            var task = this._tasks.AddTask("This is an example task");
        }

        private Tasks _tasks;

        public IEnumerable<Task> Tasks { get { return _tasks; } }

        public void AddNote(string note)
        {
            _tasks.AddTask(note);
        }

        public IEnumerable Users
        {
            get
            {
                var domains = "gmail.com,yahoo.com,hotmail.com".Split(',').ToArray();
                return new List<User>
                    (
                    Enumerable.Range(1, 100).Select(i =>
                                                    new User()
                                                    {
                                                        Id = i,
                                                        Email = "user" + i + "@" + domains[i % domains.Length],
                                                        Name = "User" + i
                                                    })
                    );
            }
        }
        public IEnumerable UsersVm
        {
            get
            {
                var domains = "gmail.com,yahoo.com,hotmail.com".Split(',').ToArray();
                return new List<User>
                    (
                    Enumerable.Range(1, 100).Select(i =>
                                                    new User()
                                                    {
                                                        Id = i,
                                                        Email = "user" + i + "@" + domains[i % domains.Length],
                                                        Name = "User" + i
                                                    })
                    ).Select(u => new UserVm{ Id = u.Id, Email = u.Email, Name = u.Name});
            }
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

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class UserVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }


    public class Tasks : IEnumerable<Task>, IHasChildren, IHasName, IHasParent<ToDoList>
    {
        private readonly List<Task> _tasks = new List<Task>();

        public Tasks(ToDoList parent)
        {
            Parent = parent;
            this._tasks = new List<Task>();
        }

        public Task AddTask(string note)
        {
            var item = new Task(this) {Text = note};
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

        public object GetChild(string name)
        {
            return _tasks.SingleOrDefault(task => task.Name == name);
        }

        public ToDoList Parent { get; private set; }

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