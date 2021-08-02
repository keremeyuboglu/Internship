using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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
        [JsonProperty(PropertyName = "geo")]
        [Required]
        public Coordinate Coordinate { get; set; }
    }

    public class Coordinate
    {
        public int Id { get; set; }
        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public string Longtitude { get; set; }
    }
}