using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Noodles.Example.Domain
{
    [Name("{Text}")]
    public class Task
    {
        public Task()
        {
            this.UniqueId = Guid.NewGuid().ToString();
        }

        public string UniqueId { get; set; }

        [Show]
        public string Text { get; set; }
        [Show]
        public bool Completed { get; set; }
        [Show]
        [DataType(DataType.Date)]
        public DateTimeOffset DueDate { get; set; }

        [Show]
        public string Status { get; set; }
        public IEnumerable<string> Status_suggestions()
        {
            return "Good,Bad,Ugly".Split(',');
        }
        [Show]
        public TaskType Type { get; set; }

    }
}