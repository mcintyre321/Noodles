using System;
using System.Collections;
using System.Collections.Generic;
using Noodles.Models;

namespace Noodles
{
    public interface IInvokeableParameter : INode, NodeProperty
    {
        object LastValue { get; set; }
        bool IsOptional { get; }
        IEnumerable Choices { get; }
        IEnumerable Suggestions { get; }
        bool Locked { get; }
    }
}