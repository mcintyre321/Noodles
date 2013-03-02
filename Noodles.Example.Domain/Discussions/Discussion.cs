using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Noodles.AspMvc.UiAttributes;
using Noodles.Example.Domain.Tasks;
using Walkies;

namespace Noodles.Example.Domain.Discussions
{
    [DisplayName("{Title}")]
    public class Discussion
    {
        public Discussion()
        {
            Messages = new List<Message>();
        }
        [ShowAsTable]
        public List<Message> Messages { get; private set; }
        
        [Show(UiOrder = 1)][Required][StringLength(30)]
        public string Title { get; set; }

        [Show]
        public void AddMessage(Message message)
        {
            Messages.Add(message.SetParent(this, Guid.NewGuid().ToString()));
        }
    }
}