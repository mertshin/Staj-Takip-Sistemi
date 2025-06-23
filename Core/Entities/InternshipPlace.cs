using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class InternshipPlace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Sector { get; set; }
        public string WorkArea { get; set; }

        public ICollection<InternshipApplication> InternshipApplications { get; set; }
    }
}
