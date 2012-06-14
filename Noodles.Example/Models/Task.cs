using System;
using System.Collections.Generic;
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

        public string Name { get; private set; }

        public string Status { get; set; }
        public IEnumerable<string> Status_choices()
        {
            return "Good,Bad,Ugly".Split(',');
        }

    }
}