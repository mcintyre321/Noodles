using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Noodles.AspMvc.UiAttributes;
using Noodles.Example.Domain.Tasks;

namespace Noodles.Example.Domain.Discussions
{
    [DisplayName("{Title}")]
    public class Discussion
    {
        public Discussion()
        {
            Messages = new List<Message>();
        }
        [ShowCollection]
        public List<Message> Messages { get; private set; }

        [Show]
        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Show]
        public void AddMessage(Message message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
                throw new UserException("You must enter a message!");
            Messages.Add(message);
        }
    }
}