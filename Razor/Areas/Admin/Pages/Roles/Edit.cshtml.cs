using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Razor.Model;

namespace Razor.Pages.Roles
{
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, RazorDb context) : base(roleManager, context)
        {
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _roleManager.Roles == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            Role = role;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var role = await _roleManager.FindByIdAsync(id);
            string oldName = role.Name;
            role.Name = Role.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Success: Role <b>{oldName}</b> updated -> <b>{role.Name}</b>";
                return RedirectToPage("./Index");
            }
            else
            {
                StatusMessage = "Error: Role not updated";
                return Page();
            }
        }

    }
}
