using System;
using System.Collections.Generic;
using Noodles.AspMvc.UiAttributes;
namespace Noodles.Example.Domain.Discussions
{
    [DisplayName("Discussions")]
    public class DiscussionsManager
    {
        public DiscussionsManager()
        {
            Discussions = new List<Discussion>();
          
        }
        [ShowCollection]
        public List<Discussion> Discussions { get; private set; }

        [Show]
        public void NewDiscussion(Discussion discussion, Message message)
        {
            Discussions.Add(discussion);
            discussion.AddMessage(message);
        }

    }

    public class Message
    {
        [Show]
        public string Text { get; set; }
    }
}
