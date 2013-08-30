using System;
using System.Collections;
using System.Collections.Generic;
using Noodles.Models;

namespace Noodles
{
    public interface IInvokeableParameter : INode
    {
        object Value { get;  }
        IEnumerable Choices { get; }
        IEnumerable Suggestions { get; }
        bool Readonly { get; }
    }
}