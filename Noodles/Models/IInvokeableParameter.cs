using System;
using System.Collections;
using System.Collections.Generic;
using Noodles.Models;

namespace Noodles
{
    public interface IInvokeableParameter : INode
    {
        string DisplayName { get; set; }
        object LastValue { get; set; }
        Type ValueType { get; }
        bool IsOptional { get; }
        string Name { get; }
        IEnumerable<Attribute> CustomAttributes { get; }
        object Value { get; }
        IEnumerable Choices { get; }
        IEnumerable Suggestions { get; }
        bool Locked { get; }
    }
}