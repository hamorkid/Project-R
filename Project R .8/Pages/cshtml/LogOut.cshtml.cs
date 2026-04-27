using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_R_._8.Pages.cshtml
{
    public class LogOutModel : PageModel
    {
        public void OnGet()
        {
            LogOut();

        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear(); 
            return RedirectToPage("/");
        }
    }
}
