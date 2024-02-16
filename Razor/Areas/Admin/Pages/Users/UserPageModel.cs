using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Model;

namespace Razor.Pages.Users;

public class UserPageModel : PageModel
{
    protected readonly RoleManager<IdentityRole> _roleManager;
    protected readonly RazorDb _context;
    protected readonly UserManager<AppUser> _userManager;
    
    [TempData]
    public string StatusMessage { get; set; }
    public UserPageModel(RoleManager<IdentityRole> roleManager, RazorDb context , UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _context = context;
        _userManager = userManager;
    }
    
}