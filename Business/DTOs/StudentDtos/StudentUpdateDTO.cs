using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.StudentDtos
{
    public class StudentUpdateDTO
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Öğrenci numarası zorunludur")]
        [StringLength(20, ErrorMessage = "Öğrenci numarası en fazla 20 karakter olabilir")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "TC Kimlik numarası zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik numarası 11 karakter olmalıdır")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik numarası sadece rakamlardan oluşmalıdır")]
        public string IdentityNumber { get; set; }

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

        [Required(ErrorMessage = "Danışman seçimi zorunludur")]
        public int AdvisorId { get; set; }
    }
}