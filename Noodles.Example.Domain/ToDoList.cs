using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Noodles.Example.Models;
using Walkies;

namespace Noodles.Example.Domain
{
    public class ToDoList : IGetChild
    {
        public ToDoList()
        {
            this._tasks = new Tasks().SetParent(this, "tasks");
            var task = this._tasks.AddTask("This is an example task");
            this.Description = "An example todo list";
        }

        [Show]
        public string Description
        {
            get;
            [DataType(DataType.MultilineText)]
            set;
        }

        [Child]
        private Tasks _tasks;

        public IEnumerable<Task> Tasks { get { return _tasks; } }

        [Show]
        public void AddTask([MyStringLength(1, 20)] string taskDescription)
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

        object IGetChild.this[string name]
        {
            get
            {
                return name == "tasks" ? _tasks : null;
            }
        }
    }
}