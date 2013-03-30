using System;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface INode
    {
        INode GetChild(string fragment);
        string Fragment { get; }
        string DisplayName { get; }
        Uri Url { get; }
        INode Parent { get; }
        //string UiHint { get; }

        //int Order { get; }
    }
}