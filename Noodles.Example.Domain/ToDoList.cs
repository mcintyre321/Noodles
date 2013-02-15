using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Walkies;

namespace Noodles.Example.Domain
{
    [DisplayName("{ListName}")]
    public class ToDoList
    {

        [Show]
        [StringLength(20)]
        [Required]
        public string ListName { get; set; }

        [Show(UiHint = "List")]
        [Collection]
        public IList<Task> Tasks { get; set; }
        public ToDoList()
        {
            Tasks = new List<Task>();
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

    }
}