using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Noodles.AspMvc
{
    public class NoodlesHub : Hub
    {
        public void Subscribe(string[] nodeUrls)
        {
            foreach (var nodeUrl in nodeUrls)
            {
                Groups.Add(Context.ConnectionId, nodeUrl);
            }
        }

        public static void NotifyClientOfChangeTo(string target)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NoodlesHub>();
            context.Clients.Group(target).onChange(new[]{target});
        }
    }
}