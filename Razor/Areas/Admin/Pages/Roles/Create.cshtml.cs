using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor.Model;

namespace Razor.Pages.Roles
{
    [Authorize(Policy = "CRUD-Role")]

    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, RazorDb context) : base(roleManager, context)
        {
        }
        
        public class InputModel
        {
            [Required]
            [Display(Name = "Role Name")]
            [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            public string RoleName { get; set; }
        }
        
        [BindProperty]
        public InputModel Input { get; set; }
        


        public async Task OnGetAsync()
        {
            
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                StatusMessage = "Error: Invalid input";
                return Page();
            }
            var oldRole = await _roleManager.FindByNameAsync(Input.RoleName);
            if(oldRole != null)
            {
                StatusMessage = $"Error: Role <b>{oldRole}</b> already exists";
                return Page();
            }
            
            var newRole = new IdentityRole(Input.RoleName);
            var result = await _roleManager.CreateAsync(newRole);
            if(result.Succeeded)
            {
                StatusMessage = "Success: Role created : " + Input.RoleName;
                return RedirectToPage("Index");
            } else
            {
                StatusMessage = "Error: Role not created";
                return Page();
            }
            
        }

        
    }
}
