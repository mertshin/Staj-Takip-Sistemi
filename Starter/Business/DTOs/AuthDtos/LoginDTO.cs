using System.ComponentModel.DataAnnotations;

namespace Business.DTOs.AuthDtos
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email gerekli")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre gerekli")]
        public string Password { get; set; } = string.Empty;
    }
} 