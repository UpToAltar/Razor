using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor.Model;

namespace Razor.Pages.Blog
{
    [Authorize(Policy = "CRUD-Blog")]
    public class IndexModel : PageModel
    {
        private readonly Razor.Model.RazorDb _context;

        public IndexModel(Razor.Model.RazorDb context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;
        public string SreachString { get; set; } = "";
        
        public int ITEMS_PER_PAGE { get; set; } = 10;
        
        public int CurrentPage { get; set; } = 1;
        
        public int CountPages { get; set; } = 10;

        public async Task OnGetAsync([FromQuery]int? page)
        {
            
            if(_context.Articles != null)
            {
                CurrentPage = page ?? 1;
                CountPages = (int)Math.Ceiling((double) await _context.Articles.CountAsync()/ ITEMS_PER_PAGE);
                Article = await _context.Articles.OrderByDescending(a => a.CreatedAt).Skip((CurrentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
            }
        }
        
        public async Task<IActionResult> OnPostAsync(string? Title)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                var list = await _context.Articles.Where(a => a.Title.Contains(Title)).ToListAsync();
                Article = list;
                SreachString = Title;
                return Page();
            }
            else
            {
                return RedirectToPage("./Index");
            }


        }
    }
}
