using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Noodles.AspMvc.UiAttributes;
using Noodles.Models;

namespace Noodles.Example.Domain
{
    public class Membership
    {
        private List<User> _users;

        public Membership()
        {
            _users = new List<User>();
        }

        [ShowAsLinkList]
        public List<User> Users
        {
            get { return _users; }
        }

        [GetChild]
        public User GetUserBySlug(string slug)
        {
            return Users.SingleOrDefault(u => Slugify(u) == slug);
        }

        private static string Slugify(User u)
        {
            return u.DisplayName.Replace(" ", "").ToLower();
        }
    }
}