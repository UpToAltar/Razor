// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor.Model;

namespace Razor.Pages.Users
{
    public class SetRole : UserPageModel
    {
        public SetRole(RoleManager<IdentityRole> roleManager, RazorDb context, UserManager<AppUser> userManager) : base(roleManager, context, userManager)
        {
        }
        
        public AppUser User { get; set; }
        
        public List<string> AllRoles { get; set; }
        
        [BindProperty]
        [Display(Name = "User Roles")]
        public List<string> UserRoles { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            User = user;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{user.Id}'.");
            }

            AllRoles = (await _roleManager.Roles.Select(r => r.Name).ToListAsync());
            UserRoles = (await _userManager.GetRolesAsync(user)).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(id);
            User = user;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{user.Id}'.");
            }
            
            // Set Role
            var Oldroles = await _userManager.GetRolesAsync(user);
            var deleteRoles = Oldroles.Except(UserRoles).ToList();
            var addRoles = UserRoles.Except(Oldroles).ToList();
            
            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                foreach (var error in resultDelete.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultAdd.Succeeded)
            {
                foreach (var error in resultAdd.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            
            StatusMessage = $"Your Roles has been set to User: {user.UserName}";
            return RedirectToPage("./Index");
        }
    }
}
