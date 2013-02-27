using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Noodles.Example.Domain.Tasks
{
    [DisplayName("{ListName}")]
    public class ToDoList
    {
        private List<Task> _tasks;

        [Show]
        [StringLength(20)]
        [Required]
        public string ListName { get; set; }

        [Show(UiHint = "Noodles/Table")]
        public IQueryable<Task> Tasks
        {
            get { return _tasks.AsQueryable(); }
        }

        public ToDoList()
        {
            _tasks= new List<Task>();
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
            return Tasks.GetEnumerator();
        }

    }
}