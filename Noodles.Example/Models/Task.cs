using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Walkies;

namespace Noodles.Example.Models
{
    [Show] 
    public class Task
    {
        public Task()
        {
            Name = Guid.NewGuid().ToString();
        }

        public string Text { get; set; }
        public bool Completed { get; set; }
    [DataType(DataType.Date)]
        public DateTimeOffset DueDate { get; set; }
        public string Name { get; private set; }

        public string Status { get; set; }
        public IEnumerable<string> Status_suggestions()
        {
            return "Good,Bad,Ugly".Split(',');
        }

        public TaskType Type { get; set; }

    }

    public enum TaskType
    {
        Task,
        Bug
    }
}