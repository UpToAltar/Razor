using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;

namespace Razor.Model;

public class AppUser : IdentityUser
{
    [Column(TypeName = "nvarchar(200)")]
    public string? Address { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? BirthDay { get; set; }
}