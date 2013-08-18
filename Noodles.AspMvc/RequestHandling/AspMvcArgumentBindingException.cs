using System.Collections.Generic;
using System.Web.Mvc;
using Noodles.RequestHandling;

namespace Noodles.AspMvc.RequestHandling
{
    internal class AspMvcArgumentBindingException : ArgumentBindingException
    {
        private readonly ModelStateDictionary _modelStateDictionary;

        public AspMvcArgumentBindingException(ModelStateDictionary modelStateDictionary)
        {
            _modelStateDictionary = modelStateDictionary;
        }

        public override IEnumerable<KeyValuePair<string, string>> Errors
        {
            get
            {
                foreach (var item in _modelStateDictionary)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        yield return new KeyValuePair<string, string>(item.Key, error.ErrorMessage ?? error.Exception.Message);
                    }
                }
            }
        }
    }
}