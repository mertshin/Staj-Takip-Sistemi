using System;
using Core.Enums;

namespace Business.DTOs.InternshipDiaryDtos
{
    public class InternshipDiaryDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string WorkDone { get; set; }
        public string Description { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string WorkHours => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        
        // Onay sistemi
        public InternshipStatus ApprovalStatus { get; set; }
        public string ApprovalStatusText => ApprovalStatus switch
        {
            InternshipStatus.Pending => "Beklemede",
            InternshipStatus.Approved => "Onaylandı",
            InternshipStatus.Rejected => "Reddedildi",
            _ => "Bilinmiyor"
        };
        public string? RejectionReason { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedByAdvisorId { get; set; }
        public string? ApprovedByAdvisorName { get; set; }

        // İlişkiler
        public int InternshipApplicationId { get; set; }
        public string StudentName { get; set; }
        public string StudentNumber { get; set; }
        public string InternshipTopic { get; set; }
    }
} 