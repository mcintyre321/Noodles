using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [Name("{Description}")]
    public class ToDoList : List<Task>
    {
        [Show]
        public IList<Task> Tasks = new List<Task>();
        public ToDoList()
        {
            this.Tasks = new List<Task>();
        }

        [Show]
        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        [Show]
        public void ClearCompletedTasks()
        {
            this.Tasks.Where(t => t.Completed).ToList().ForEach(t => Tasks.Remove(t));
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return Tasks.GetEnumerator();
        }

        [Show]
        public string Description { get; set; }
    }
}