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
    public static class InvokeableParameterExt
    {
        public static NodeMethod QueryChoices(this IInvokeableParameter parameter)
        {
            return parameter.Parent.GetChild(parameter.Name + "_QueryChoices") as NodeMethod;
        }
        public static NodeMethod QuerySuggestions(this IInvokeableParameter parameter)
        {
            return parameter.Parent.GetChild(parameter.Name + "_QuerySuggestions") as NodeMethod;
        }

    }
}