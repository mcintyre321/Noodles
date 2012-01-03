using System;
using System.Collections.Generic;
using WebNoodle.Reflection;

namespace WebNoodle
{
    public interface INode : IBehaviour
    {
        string Path { get; }
        string Name { get; }
        string Id { get; }
        Type NodeType { get; }
        INode GetChild(IEnumerator<string> pathEnumerator);
    }

    public interface IBehaviour
    {
        IEnumerable<IObjectMethod> NodeMethods();
    }
}