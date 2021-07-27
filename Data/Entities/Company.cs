using Microsoft.EntityFrameworkCore;

namespace Altamira.Data.Entities
{
    public class Company
    {
        // Surrogate key and properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        public string Business { get; set; }
    }
}