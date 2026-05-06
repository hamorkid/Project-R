using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_R_._8.Pages.cshtml
{
    public class HomePageModel : PageModel
    {
        public string Username { get; set; } = "";
        public string DisplayName { get; set; } = "";

        public string WelcomeMessage { get; set; } = "";
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                WelcomeMessage = "You are not logged in. <br>please log in to access all the features of the website.";
                return Page();
            }
            Username = HttpContext.Session.GetString("Username")!;
            DisplayName = HttpContext.Session.GetString("DisplayName")!;
            WelcomeMessage  =  $"Welcome, {DisplayName}! <br>You Are Logged In As @{Username}. <br>Have Fun Playing Around!";
            return Page();

        }
    }
}
