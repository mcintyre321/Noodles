using System;
using System.Collections.Generic;
using Walkies;
namespace Noodles.Example.Domain.Discussions
{
    [DisplayName("Discussions")]
    public class DiscussionsManager
    {
        public DiscussionsManager()
        {
            Discussions = new List<Discussion>();
            var discussion = new Discussion()
            {
                Title = "My opinion",
            };
            
            NewDiscussion(discussion, new Message(){ Text = "We should all do exactly what I say."});
        }
        [Show(UiHint = "Noodles/LinkList.")]
        public List<Discussion> Discussions { get; private set; }

        [Show]
        public void NewDiscussion(Discussion discussion, Message message)
        {
            Discussions.Add(discussion.SetParent(Discussions, Guid.NewGuid().ToString()));
            discussion.AddMessage(message);
        }

    }

    public class Message
    {
        [Show]
        public string Text { get; set; }
    }
}
