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

namespace Razor.Pages.Users
{
    public class IndexModel : UserPageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, RazorDb context, UserManager<AppUser> userManager) : base(roleManager, context, userManager)
        {
        }

        public int totalUsers { get; set; } = 0;
        
        public class UserAndRoles : AppUser
        {
            public List<string> UserRoles { get; set; }
        }
        public List<UserAndRoles> Users { get; set; } = default!;
        
        public List<string> Roles { get; set; } = default!;
        
        public int ITEMS_PER_PAGE { get; set; } = 10;
        
        public int CurrentPage { get; set; } = 1;
        
        public int CountPages { get; set; } = 10;

        public static string getAddress(string[] address)
        {
            int count = address.Length;
            int random = new Random().Next(0, count);
            return address[random];
        }
        
        public async Task<IActionResult> OnGetAsync([FromQuery] int? page)
        {
            // Seed Users
            totalUsers = await _userManager.Users.CountAsync();
            if (totalUsers < 20)
            {
                for (int i = 1; i <= 150; i++)
                {
                    await _userManager.CreateAsync(new AppUser
                    {
                        UserName = $"User-{i}",
                        Email = $"User-{i}@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "1234567890",
                        PhoneNumberConfirmed = true,
                        TwoFactorEnabled = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        Address = getAddress(new string[] {"Ha Noi", "Da Nang", "HCM"})
                    }, "Password@123");
                }
            } 
            
            if(_context.Users != null)
            {
                CurrentPage = page ?? 1;
                CountPages = (int)Math.Ceiling((double) await _userManager.Users.CountAsync()/ ITEMS_PER_PAGE);
                Users = await _context.Users.OrderBy(u => u.UserName)
                    .Skip((CurrentPage - 1) * ITEMS_PER_PAGE)
                    .Take(ITEMS_PER_PAGE)
                    .Select(u => new UserAndRoles()
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Address = u.Address,
                    }).ToListAsync();
                foreach (var user in Users)
                {
                    user.UserRoles = (await _userManager.GetRolesAsync(user)).ToList();
                }
                
            }
            
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            
            return Page();
        }
    }
}
