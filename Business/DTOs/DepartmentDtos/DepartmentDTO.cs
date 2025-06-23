using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.DepartmentDtos
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public int StudentCount { get; set; }
        public int AdvisorCount { get; set; }
    }
}
