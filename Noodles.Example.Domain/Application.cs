using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noodles.Example.Models;
using Walkies;

namespace Noodles.Example.Domain
{
    public class Application
    {
        public Application()
        {
            ToDoLists = new ToDoLists();
        }
        [Child]
        public ToDoLists ToDoLists { get; private set; }
        
    }
}
