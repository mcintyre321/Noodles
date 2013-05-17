using System;
using System.Collections.Generic;
using System.Linq;


namespace Noodles.Models
{
    public class QueryPage 
    {
        private readonly IQueryable<object> _fetch;
        private readonly INode _parent;
        public int Skip { get; set; }
        public int Take { get; set; }

        public IEnumerable<Resource> Items
        {
            get
            {
                return
                    _fetch.Skip(Skip).Take(Take).ToArray().Select(
                        (o, i) => ResourceFactory.Instance.Create(o, _parent, (Skip + i).ToString()));
            }
        }

        public QueryPage(int skip, int take, IQueryable<object> fetch, INode parent)
        {
            _fetch = fetch;
            _parent = parent;
            Skip = skip;
            Take = take;
        }
         
    }
}