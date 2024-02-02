using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor.Model;

namespace Razor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly RazorDb _razorDb;

    public IndexModel(ILogger<IndexModel> logger, RazorDb razorDb)
    {
        _logger = logger;
        _razorDb = razorDb;
    }
    
    public IList<Article> Articles { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_razorDb.Articles != null)
        {
            Articles= await _razorDb.Articles.ToListAsync();
        }
    }

    
}