using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Altamira.Data.Entities
{
    public class Company
    {
        // Surrogate key and properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        [JsonProperty(PropertyName = "bs")]
        public string Business { get; set; }
    }
}