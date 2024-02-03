using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Model;

namespace Razor.Pages.Roles;

public class RolePageModel : PageModel
{
    protected readonly RoleManager<IdentityRole> _roleManager;
    protected readonly RazorDb _context;
    
    [TempData]
    public string StatusMessage { get; set; }
    public RolePageModel(RoleManager<IdentityRole> roleManager, RazorDb context)
    {
        _roleManager = roleManager;
        _context = context;
    }
}