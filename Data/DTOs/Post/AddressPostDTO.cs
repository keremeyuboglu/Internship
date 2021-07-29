using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.DTOs.Post
{
    public class AddressPostDTO
    {

        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        [JsonProperty(PropertyName = "geo")]
        public CoordinatePostDTO Coordinate { get; set; }
    }

    public class CoordinatePostDTO
    {
        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public string Longtitude { get; set; }
    }
}
