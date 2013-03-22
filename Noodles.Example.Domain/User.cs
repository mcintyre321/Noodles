using System.ComponentModel.DataAnnotations;

namespace Noodles.Example.Domain
{
    public class User
    {
        [Show][Required]
        public string DisplayName { get; set; }
        [Show][Required][DataType(DataType.Password)]
        public string Password { get; set; }
        [Show][Required][DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}