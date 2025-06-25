using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Entities
{
    public class InternshipDiary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? WorkDone { get; set; }
        public string? Description { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        // Onay sistemi
        public InternshipStatus ApprovalStatus { get; set; } = InternshipStatus.Pending;
        public string? RejectionReason { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedByAdvisorId { get; set; }
        public Advisor? ApprovedByAdvisor { get; set; }

        public int InternshipApplicationId { get; set; }
        public InternshipApplication? InternshipApplication { get; set; }
    }
}
