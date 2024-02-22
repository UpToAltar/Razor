using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Model;

namespace Razor.Pages.Roles;

[Authorize(Policy = "CRUD-Role")]
public class EditClaim : RolePageModel
{
    
    public EditClaim(RoleManager<IdentityRole> roleManager, RazorDb context) : base(roleManager, context)
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
    
    public IdentityRoleClaim<string> RoleClaim { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int? claimid)
    {
        if (claimid == null || _context.RoleClaims == null)
        {
            return NotFound();
        }
        // Find claim
        var claim = await _context.RoleClaims.FindAsync(claimid);
            
        if (claim == null)
        {
            return NotFound();
        }
        RoleClaim = claim;
        
        // Find role
        var role = await _roleManager.FindByIdAsync(claim.RoleId);
        
        if (role == null)
        {
            return NotFound();
        }
        
        Role = role;
        InputModel input = new InputModel()
        {
            ClaimValue = RoleClaim.ClaimValue,
            ClaimType = RoleClaim.ClaimType
        };
        Input = input;
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int? claimid)
    {
        if(!ModelState.IsValid)
        {
            StatusMessage = "Error: Invalid input";
            return Page();
        }
        // Find claim
        var claim = await _context.RoleClaims.FindAsync(claimid);
            
        if (claim == null)
        {
            return NotFound();
        }
        
        // Find role
        var role = await _roleManager.FindByIdAsync(claim.RoleId);
        if(role == null)
        {
            StatusMessage = $"Error: Role <b>{role}</b> not exists";
            return RedirectToPage("EditClaim", new {claimid = claim.Id});
        }
        //Check if claim exists
        var checkClaim = (await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == Input.ClaimType && c.Value == Input.ClaimValue);
        if(checkClaim)
        {
            StatusMessage = $"Error: Claim <b>{Input.ClaimType} = {Input.ClaimValue}</b> already exists";
            return RedirectToPage("EditClaim", new {claimid = claim.Id});
        }
        // Update claim
        claim.ClaimType = Input.ClaimType;
        claim.ClaimValue = Input.ClaimValue;
        await _context.SaveChangesAsync();
        
        StatusMessage = "Success: Claim has been updated for role : " + role.Name;
        return RedirectToPage("Edit", new {id = role.Id});
        }
        
    public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
    {
        if(!ModelState.IsValid)
        {
            StatusMessage = "Error: Invalid input";
            return Page();
        }
        // Find claim
        var claim = await _context.RoleClaims.FindAsync(claimid);
            
        if (claim == null)
        {
            return NotFound();
        }
        // Find role
        var role = await _roleManager.FindByIdAsync(claim.RoleId);
        if(role == null)
        {
            StatusMessage = $"Error: Role <b>{role}</b> not exists";
            return NotFound();
        }
        // delete claim
         await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));
        
        StatusMessage = $"Success: Claim of {role.Name} has been deleted";
        return RedirectToPage("Edit", new {id = role.Id});
    }
}

    
    
