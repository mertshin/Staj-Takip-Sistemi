using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Entities
{
    public class InternshipApplication
    {
        public int Id { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string? InternshipTopic { get; set; }
        public InternshipStatus Status { get; set; }
        public string? RejectionReason { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int AdvisorId { get; set; }
        public Advisor? Advisor { get; set; }

        public int? InternshipPlaceId { get; set; }
        public InternshipPlace? InternshipPlace { get; set; }

        public ICollection<InternshipDiary>? InternshipDiaries { get; set; }
        public InternshipEvaluation? Evaluation { get; set; }
    }
}
