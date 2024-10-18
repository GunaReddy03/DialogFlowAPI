using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DialogFlowAPI.Models
{
    public class Logins
    {
        public class RegisterModel
        {
            [Required(ErrorMessage = "User Name is required")]
            public string? Username { get; set; }

            [EmailAddress]
            [Required(ErrorMessage = "Email is required")]
            public string? Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string? Password { get; set; }

            public string? JobTitle { get; set; }

            public string? FullName { get; set; }

            public string? Configuration { get; set; }

            public bool IsEnabled { get; set; }

            public string? Country { get; set; }

            public string? State { get; set; }

            public string? City { get; set; }

            public string? Address { get; set; }
        }
        public class LoginModel
        {
            [Required(ErrorMessage = "User Name is required")]
            public string? Username { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string? Password { get; set; }
        }
        public class Response
        {
            public string? Status { get; set; }
            public string? Message { get; set; }
        }
        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string User = "User";
            public const string Agent = "Agent";
            public const string Customer = "Customer";
        }

        public class ApplicationUser : IdentityUser
        {


            public string? AccountType { get; set; }

            public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

            public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        }
    }
}
