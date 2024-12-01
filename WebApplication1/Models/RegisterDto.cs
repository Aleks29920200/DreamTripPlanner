using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class RegisterDto:IdentityUser
    {

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [PasswordPropertyText]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
    }
}
