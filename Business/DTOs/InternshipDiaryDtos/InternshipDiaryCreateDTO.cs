using System;
using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.InternshipDiaryDtos
{
    public class InternshipDiaryCreateDTO
    {
        [Required(ErrorMessage = "Tarih gerekli")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Yapılan çalışma alanı gerekli")]
        [MaxLength(500, ErrorMessage = "Yapılan çalışma en fazla 500 karakter olabilir")]
        public string WorkDone { get; set; }

        [Required(ErrorMessage = "Açıklama gerekli")]
        [MaxLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gerekli")]
        public TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0); // 09:00

        [Required(ErrorMessage = "Bitiş saati gerekli")]
        public TimeSpan EndTime { get; set; } = new TimeSpan(17, 0, 0); // 17:00

        [Required(ErrorMessage = "Staj başvuru ID gerekli")]
        public int InternshipApplicationId { get; set; }
    }
} 