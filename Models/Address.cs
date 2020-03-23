using System;
using System.ComponentModel.DataAnnotations;

namespace GSAPP.Models {
    public class Address {
        [Key]
        public int AdressId { get; set; }

        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int ZipCode { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        public virtual User User { get; set; }
        // joining one to one to User  
        public DateTime CreatedAt = DateTime.Now;
        public DateTime UpdatedAt = DateTime.Now;
    }
}