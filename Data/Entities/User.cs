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
        // non-Surrogate key and properties
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public string Password { get; set; } = "p@ssword."; // DUMMY DEFAULT PASSWORD
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Website { get; set; }
        // foreign keys
        public Address Address { get; set; }
        public Company Company { get; set; }
    }
}
