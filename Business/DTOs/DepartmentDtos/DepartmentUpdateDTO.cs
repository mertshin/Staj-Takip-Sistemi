using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.DepartmentDtos
{
    public class DepartmentUpdateDTO
    {
        [Required(ErrorMessage = "Bölüm adı zorunludur")]
        [StringLength(100, ErrorMessage = "Bölüm adı en fazla 100 karakter olabilir")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bölüm kodu zorunludur")]
        [StringLength(20, ErrorMessage = "Bölüm kodu en fazla 20 karakter olabilir")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Üniversite seçimi zorunludur")]
        public int UniversityId { get; set; }
    }
}
