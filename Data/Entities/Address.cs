using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Altamira.Data.Entities
{
    public class Address
    {
        // Surrogate key and properties
        public int Id { get; set; }
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public Point Coordinate { get; set; }
    }
}