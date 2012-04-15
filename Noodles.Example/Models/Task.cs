using System;
using System.Collections.Generic;

namespace Noodles.Example.Models
{
    public class Task
    {
        [Parent]
        private readonly Tasks _parent;

        public Task(Tasks parent)
        {
            _parent = parent;
            Name = Guid.NewGuid().ToString();
        }

        public string Text { get; set; }
        public bool Completed { get; set; }

        [Name]
        public string Name { get; private set; }

        public string Status { get; [Show] set; }
        public IEnumerable<string> Status_choices()
        {
            return "Good,Bad,Ugly".Split(',');
        }

    }
}