using System.ComponentModel.DataAnnotations;
using Noodles.Models;

namespace Noodles.Example.Domain
{
    public class User
    {
        [Show][Required]
        public string DisplayName { get; set; }

        [Slug]
        public string Slug { get { return DisplayName.Replace(" ", "").ToLower(); } }
        
        [Show][Required][DataType(DataType.Password)]
        public string Password { get; set; }

        [Show][Required][DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}