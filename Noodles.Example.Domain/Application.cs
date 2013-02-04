using Walkies;

namespace Noodles.Example.Domain
{
    [Name("To Do List Application")]
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
