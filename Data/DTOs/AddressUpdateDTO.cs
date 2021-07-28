using Altamira.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.DTOs
{
    public class AddressUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Suite { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Zipcode { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
