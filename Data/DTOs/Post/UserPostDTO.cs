using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.DTOs.Post
{
    public class UserPostDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        // foreign keys
        public AddressPostDTO Address { get; set; }
        public CompanyPostDTO Company { get; set; }
    }
}
