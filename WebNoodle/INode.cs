using System.Collections.Generic;
using WebNoodle.Reflection;

namespace WebNoodle
{
    public interface INode : IBehaviour
    {
        string Path { get; }
        string Name { get; }
        string Id { get; }
        INode GetChild(string name);
    }

    public interface IBehaviour
    {
        IEnumerable<IObjectMethod> NodeMethods();
    }
}