using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public int UniversityId { get; set; }
        public University University { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Advisor> Advisors { get; set; }
    }
}
