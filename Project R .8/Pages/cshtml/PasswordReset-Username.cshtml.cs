using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_R_._8.Pages.cshtml
{
    public class PasswordReset_UsernameModel : PageModel
    {
        public string ErrorMessage { get; set; } = "";
        [BindProperty] public string Username { get; set; } = "";
        private readonly DBHelper _db;

        public PasswordReset_UsernameModel(DBHelper db)
        {
            _db = db;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {    
            DataTable user = _db.GetData(
                $"SELECT * FROM Users WHERE UserName = '{Username}'"
            );

            if (user.Rows.Count == 0)
            {
                ErrorMessage = "Invalid username";
                return Page();
            }

            TempData["Username"] = Username;
            return RedirectToPage("/cshtml/PasswordReset-Answers")
        }
    }
}
