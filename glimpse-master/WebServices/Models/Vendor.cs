using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServices.Models
{
    public class Vendor
    {
        public Vendor()
        {
            this.Promotions = new List<Promotion>();
        }

        public int VendorId { get; set; }      

        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string CompanyName { get; set; }

        public string Salt { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        [NotMapped]
        public bool RequestFromWeb { get; set; }

        [Index(IsUnique = true)]
        public Location Location { get; set; }

        public virtual ICollection<Promotion> Promotions { get; set; }
    }
}