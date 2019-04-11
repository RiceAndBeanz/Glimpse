using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServices.Models
{
    public class User
    {
        public int UserId { get; set; }      

        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

    }
}