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
    [Authorize(Policy = "Read-Role")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, RazorDb context) : base(roleManager, context)
        {
        }

        public class RoleModel : IdentityRole
        {
            public List<string> Claims { get; set; }
        }
        public List<RoleModel> Roles { get; set; }
        
        

        public async Task OnGetAsync()
        {
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            Roles = new List<RoleModel>();
            foreach (var role in roles)
            {
                var roleModel = new RoleModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Claims = (await _roleManager.GetClaimsAsync(role)).Select(c => $"{c.Type} = {c.Value}").ToList()
                };
                Roles.Add(roleModel);
            }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }

        
    }
}
