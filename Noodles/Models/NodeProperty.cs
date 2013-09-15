using System;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface NodeProperty : IInvokeableParameter
    {
        void SetValue(object value);
        Resource GetResource();
    }

    public interface NodeCollectionProperty : Resource
    {
        QueryPage Query(int skip, int take);
    }
}