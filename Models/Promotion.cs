using System.Collections.Generic;

namespace AlumniConnect.API.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public ICollection<AlumniUser> AlumniUsers { get; set; }
    }
}
