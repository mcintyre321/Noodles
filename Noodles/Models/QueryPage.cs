using System;
using System.Collections.Generic;
using System.Linq;
using Walkies;

namespace Noodles.Models
{
    public class QueryPage 
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<Resource> Items { get; private set; }

        public QueryPage(int skip, int take, IEnumerable<Resource> fetch)
        {
            Skip = skip;
            Take = take;
            Items = fetch.ToArray();
        }
         
    }
}