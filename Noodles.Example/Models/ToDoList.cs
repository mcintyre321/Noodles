using System.Collections.Generic;
using System.Linq;
using Noodles.Web.Helpers.Icons;
using Walkies;

namespace Noodles.Example.Models
{
    public class ToDoList : IGetChild
    {
        public ToDoList()
        {
            this._tasks = new Tasks().SetParent(this, "tasks");
            var task = this._tasks.AddTask("This is an example task");
        }
        
        [Child]
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
         

        [Show][Icon("minus-sign", "white")]
        public void ClearCompletedTasks()
        {
            _tasks.RemoveComplete();
        }

        object IGetChild.this[string name]
        {
            get
            {
                return name == "tasks" ? _tasks : null;
            }
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
}