using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Noodles.Example.Models
{
    public class ToDoList : IHasChildren
    {
        public ToDoList()
        {
            this._tasks = new Tasks(this);
            var task = this._tasks.AddTask("This is an example task");
        }

        //[Child]
        private Tasks _tasks;

        public IEnumerable<Task> Tasks { get { return _tasks; } }
        [Show]
        public void AddTask(string taskDescription)
        {
            if (string.IsNullOrWhiteSpace(taskDescription)) throw new UserException("Task description cannot be empty");
            _tasks.AddTask(taskDescription);
        }

        public IEnumerable<User> Users
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
         

        [Show]
        public void ClearCompletedTasks()
        {
            _tasks.RemoveComplete();
        }

        public object GetChild(string name)
        {
            if (name == "tasks") return _tasks;
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

    [Show]
    public class Tasks : IEnumerable<Task>
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

        [Parent]
        public ToDoList Parent { get; private set; }

        [Name]
        public string Name { get { return "tasks"; } }

        public void RemoveComplete()
        {
            this._tasks.RemoveAll(t => t.Completed);
        }
    }
}