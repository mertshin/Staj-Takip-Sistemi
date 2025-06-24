using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.AdvisorDtos
{
    public class AdvisorUpdateDTO
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Unvan zorunludur")]
        [StringLength(50, ErrorMessage = "Unvan en fazla 50 karakter olabilir")]
        public string Title { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon zorunludur")]
        [StringLength(15, ErrorMessage = "Telefon en fazla 15 karakter olabilir")]
        [RegularExpression(@"^[0-9+\-\s\(\)]+$", ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Bölüm seçimi zorunludur")]
        public int DepartmentId { get; set; }
    }
}