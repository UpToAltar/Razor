using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor.Model;

namespace Razor.Pages.Roles
{
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, RazorDb context) : base(roleManager, context)
        {
        }

        public List<IdentityRole> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }

        
    }
}
