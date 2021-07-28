using Altamira.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.DTOs
{
    public class UserUpdateDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        // foreign keys
        public AddressUpdateDTO Address { get; set; }
        public CompanyUpdateDTO Company { get; set; }
    }
}
