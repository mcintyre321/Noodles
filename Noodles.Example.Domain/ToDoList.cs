using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [Name("{ListName}")]
    public class ToDoList
    {
        [Show(UiHint = "List")]
        public IList<Task> Tasks = new List<Task>();
        public ToDoList()
        {
            this.Tasks = new List<Task>().SetParent(this, "Tasks").SetName("Tasks");
        }

        [Show]
        public void AddTask(Task task)
        {
            Tasks.Add(task.SetParent(Tasks, Guid.NewGuid().ToString()));
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

        [Show][MyStringLength(1, 20)] 
        public string ListName { get; set; }
    }
}