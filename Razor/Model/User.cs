using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Razor.Validation;

namespace Razor.Model;

public class User
{
    [DisplayName("UserName")]
    [Required]
    [StringLength(20, MinimumLength = 6 , ErrorMessage = "Name must be between {1} and {2} characters")]
    public string Name { get; set; }
    
    [DisplayName("Email")]
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [DisplayName("Password")]
    [Required]
    [CheckPass]
    public string Password { get; set; }
    
    [DisplayName("Confirm Password")]
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}