using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.Entities
{
    public class User
    {
        // Surrogate key and properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public string Password { get; set; } = "p@ssword."; // DUMMY DEFAULT PASSWORD
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.Url)]
        public string Website { get; set; }
        // foreign keys
        [Required]
        public Address Address { get; set; }
        [Required]
        public Company Company { get; set; }
    }
}
