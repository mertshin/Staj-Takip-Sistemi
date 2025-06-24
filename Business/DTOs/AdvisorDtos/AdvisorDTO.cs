using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.AdvisorDtos
{
    public class AdvisorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{Title} {FirstName} {LastName}";
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string UniversityName { get; set; }

        public int StudentCount { get; set; }
        public int InternshipApplicationCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}