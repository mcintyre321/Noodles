using System;
using System.Reflection;

namespace Noodles.Models
{
    public interface NodeLink : IHasName
    {
        Resource Target { get; }
        string DisplayName { get; }
        Type TargetType { get; }
        string UiHint { get; }
        Uri Url { get; }
    }
}