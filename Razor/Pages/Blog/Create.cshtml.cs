using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Razor.Model;

namespace Razor.Pages.Blog
{
    [Authorize(Policy = "CRUD-Blog")]
    public class CreateModel : PageModel
    {
        private readonly Razor.Model.RazorDb _context;

        public CreateModel(Razor.Model.RazorDb context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Articles == null || Article == null)
            {
                return Page();
            }
            Console.WriteLine( Article);
          await _context.Articles.AddAsync(Article);
          await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
