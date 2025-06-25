using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Business.DTOs.AuthDtos
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Ad gerekli")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad gerekli")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email gerekli")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre gerekli")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı gerekli")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rol seçimi gerekli")]
        public UserRole Role { get; set; }
    }
} 