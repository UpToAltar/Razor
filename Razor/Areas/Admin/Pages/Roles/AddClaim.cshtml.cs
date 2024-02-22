using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Model;

namespace Razor.Pages.Roles;

[Authorize(Policy = "CRUD-Role")]
public class AddClaim : RolePageModel
{
    public AddClaim(RoleManager<IdentityRole> roleManager, RazorDb context) : base(roleManager, context)
    {
    }
    public class InputModel
    {
        [Required]
        [Display(Name = "Claim Type")]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string ClaimType { get; set; }
        
        [Required]
        [Display(Name = "Claim Value")]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string ClaimValue { get; set; }
    }
    
    [BindProperty]
    public InputModel Input { get; set; }
    
    public IdentityRole Role { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string id)
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
    
    public async Task<IActionResult> OnPostAsync(string id)
    {
        if(!ModelState.IsValid)
        {
            StatusMessage = "Error: Invalid input";
            return Page();
        }
        // Find role
        var role = await _roleManager.FindByIdAsync(id);
        if(role == null)
        {
            StatusMessage = $"Error: Role <b>{role}</b> not exists";
            return RedirectToPage("AddClaim", new {id = role.Id});
        }
        //Check if claim exists
        var checkClaim = (await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == Input.ClaimType && c.Value == Input.ClaimValue);
        if(checkClaim)
        {
            StatusMessage = $"Error: Claim <b>{Input.ClaimType} = {Input.ClaimValue}</b> already exists";
            return RedirectToPage("AddClaim", new {id = role.Id});
        }
        // Add claim
        var result =  await _roleManager.AddClaimAsync(role, new Claim(Input.ClaimType, Input.ClaimValue));
        if(result.Succeeded)
        {
            StatusMessage = "Success: Claim has been created for role : " + role.Name;
            return RedirectToPage("Edit", new {id = role.Id});
        } else
        {
            StatusMessage = "Error: Claim not created";
            return RedirectToPage("AddClaim", new {id = role.Id});
        }
        }
        
}

    
    
