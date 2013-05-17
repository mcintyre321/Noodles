using System.Collections;
using System.Collections.Generic;
using Noodles.Models;

namespace Noodles.RequestHandling.ResultTypes
{
    public class ValidationErrorResult : Result, IEnumerable<KeyValuePair<string, string>>
    {
        private List<KeyValuePair<string, string>> _messages = new List<KeyValuePair<string, string>>();
        private IInvokeable _invokeable;

        public ValidationErrorResult(IInvokeable invokeable)
        {
            _invokeable = invokeable;
        }

        public IInvokeable Invokeable
        {
            get { return _invokeable; }
        }

        public void Add(string key, string value)
        {
            this._messages.Add(new KeyValuePair<string, string>(key, value));
        }
        public void Add(IEnumerable<KeyValuePair<string, string>> items)
        {

            this._messages.AddRange(items);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _messages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}