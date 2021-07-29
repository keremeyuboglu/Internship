using Altamira.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.DTOs.Update
{
    public class AddressUpdateDTO
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        [JsonProperty(PropertyName = "geo")]
        public CoordinateUpdateDTO Coordinate { get; set; }
    }

    public class CoordinateUpdateDTO
    {
        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public string Longtitude { get; set; }
    }
}
