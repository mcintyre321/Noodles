using System;
using System.Security.Policy;

namespace Noodles.Models
{
    public interface NodeMethod : INode, IInvokeable
    {
        string UiHint { get; }
    }

    public class NotEnoughParametersForNodeMethodException : Exception
    {
        public NodeMethod NodeMethod { get; set; }
        public IInvokeableParameter ParameterInfo { get; set; }
        public object[] Parameters { get; set; }

        public NotEnoughParametersForNodeMethodException(NodeMethod nodeMethod, IInvokeableParameter parameterInfo, object[] parameters)
        {
            NodeMethod = nodeMethod;
            ParameterInfo = parameterInfo;
            Parameters = parameters;
        }
         
    }
}