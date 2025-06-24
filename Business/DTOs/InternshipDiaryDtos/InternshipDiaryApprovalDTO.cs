using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Business.DTOs.InternshipDiaryDtos
{
    public class InternshipDiaryApprovalDTO
    {
        public int DiaryId { get; set; }
        
        [Required(ErrorMessage = "Karar seçimi gerekli")]
        public InternshipStatus Decision { get; set; }
        
        [MaxLength(500, ErrorMessage = "Red sebebi en fazla 500 karakter olabilir")]
        public string? RejectionReason { get; set; }
        
        public int AdvisorId { get; set; }
    }
} 