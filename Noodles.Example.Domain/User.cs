using System.ComponentModel.DataAnnotations;
using Noodles.Models;

namespace Noodles.Example.Domain
{
    [DisplayName("{DisplayName}")]
    public class User
    {
        private string _slug; //don't change the slug after it has been initially set

        [Show][Required]
        public string DisplayName { get; set; }

        [Slug]
        public string Slug { get { return _slug ?? (_slug = DisplayName.Replace(" ", "").ToLower()); } }
        
        [Show][Required][DataType(DataType.Password)]
        public string Password { get; set; }

        [Show][Required][DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}