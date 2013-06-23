using System;
using System.Collections.Generic;
using System.Linq;

namespace Noodles.Models
{
    public class BaseNodeProperty : BaseNode, NodeProperty
    {
        public BaseNodeProperty(string name, INode parent, Type valueType)
            :base(name, parent)
        {
          
            ValueType = valueType;
            ParameterType = valueType;
            ResultType = valueType;
        }
      

        public int Order
        {
            get
            {
                return CustomAttributes.OfType<ShowAttribute>().Select(a => a.UiOrder as int?).SingleOrDefault() ?? int.MaxValue;
            }
        }
        public Type ValueType { get; private set; }
        public bool Readonly { get { return DoInvoke == null; } }
        public object Value { get; set; }

        public virtual IEnumerable<object> CustomAttributes { get{ yield break;} }
        public virtual IEnumerable<object> GetterCustomAttributes { get { yield break; } }
        public virtual IEnumerable<object> SetterCustomAttributes { get { yield break; } }

        public IEnumerable<IInvokeableParameter> Parameters { get; private set; }
        public string InvokeDisplayName { get { return "Set " + this.DisplayName; } }

        public Uri InvokeUrl
        {
            get { return this.Url; }
        }

        public object Target { get; protected set; }
        public string Message { get; private set; }
        public Type ParameterType { get; private set; }
        public Type ResultType { get; private set; }

        public bool Active { get { return Readonly; } }

        public object Invoke(IDictionary<string, object> parameterDictionary)
        {
            return Invoke(new[] {parameterDictionary.Values.SingleOrDefault()});
        }

        public virtual object Invoke(object[] parameters)
        {
            return DoInvoke(this, parameters);
        }

        public T GetAttribute<T>() where T : Attribute
        {
            return this.CustomAttributes.OfType<T>().SingleOrDefault();
        }

        public Func<NodeProperty, object[], object> DoInvoke { get; set; }
        
        public IEnumerable<NodeMethod> NodeMethods { get { yield break; } }
        public virtual string UiHint { get; set; }

       
    }

    public class BaseNode : INode
    {
        protected BaseNode(string name, INode parent)
        {
            Name = name;
            DisplayName = name.Sentencise(titlecase: true);
            Parent = parent;

        }

        public string Name { get; private set; }
 
        public virtual INode GetChild(string name)
        {
            return null;
        }
        public virtual string DisplayName { get; protected set; }
        public Uri Url
        {
            get { return new Uri((this.Parent == null ? "/" : this.Parent.Url.ToString()) + this.Name + "/", UriKind.Relative); }
        }
        public INode Parent { get; private set; }
    }
}