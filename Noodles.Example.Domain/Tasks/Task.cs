using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Noodles.Example.Domain.Tasks
{
    [DisplayName("{Title}")]
    public class Task
    {
        [Required][StringLength(30)]
        public string Title { get; set; }

        [Show]
        public string Notes { get; set; }

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