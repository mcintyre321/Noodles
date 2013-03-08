using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Noodles.Example.Domain
{
    public class Membership
    {
        private List<User> _users;

        public Membership()
        {
            _users = new List<User>();
        }

        [Show]
        public IEnumerable<User> Users
        {
            get { return _users; }
        }

        
    }
}