using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor.Model;

namespace Razor.Pages;

public class FormModel : PageModel
{
    [BindProperty]
    public User UserInfo { set; get; } 
    public string Error { set; get; }
    public void OnPost()
    {
        if(ModelState.IsValid)
        {
            Error = "Ok Input";
        }
        else
        {
            Error = "Invalid Input";
        }
    }
}