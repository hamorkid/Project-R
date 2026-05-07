using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_R_._8.Pages.cshtml
{
    public class PasswordReset_CreatePasswordModel : PageModel
    {
        private readonly DBHelper _db;

        public PasswordReset_CreatePasswordModel(DBHelper db)
        {
            _db = db;
        }

        [BindProperty] public string Password { get; set; } = "";
        [BindProperty] public string RePassword { get; set; } = "";
        
        public string ErrorMessage { get; set; } = "";

        

        public void OnGet()
        {

        }

        public IActionResult OnPost() 
        {
            if (Password != RePassword)
            {
                ErrorMessage = "Passwords do not match";
                return Page();
            }

            string PasswordHash = HashPassword(Password);

            _db.ExecuteQuery(
                @"UPDATE Users
                SET PasswordHash = @PasswordHash
                WHERE UserName = @Username",
                new {PasswordHash = PasswordHash, Username = TempData["Username"]!.ToString()}
            );

            return RedirectToPage("/cshtml/HomePage");
        }


        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
