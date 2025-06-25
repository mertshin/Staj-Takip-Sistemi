using Microsoft.AspNetCore.Identity;
using Core.Enums;

namespace Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; } = false; // Admin onayı gerekiyor
        public DateTime? LastLoginDate { get; set; }
        
        // Navigation Properties
        public Student? Student { get; set; }
        public Advisor? Advisor { get; set; }
    }
} 