using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_R_._8.Services;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Project_R_._8.Pages.cshtml
{
    public class LoginModel : PageModel
    {
        private readonly DBHelper _db;

        public LoginModel(DBHelper db)
        {
            _db = db;
        }

        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string passwordHash = HashPassword(Password);

            DataTable user = _db.GetData(
                $"SELECT * FROM Users WHERE UserName = '{Username}' AND PasswordHash = '{passwordHash}'"
            );

            if (user.Rows.Count == 0)
            {
                ErrorMessage = "Invalid username or password";
                return Page();
            }

            // Save user info in session
            HttpContext.Session.SetString("UserId", user.Rows[0]["UserId"].ToString()!);
            HttpContext.Session.SetString("Username", user.Rows[0]["UserName"].ToString()!);
            HttpContext.Session.SetString("IsAdmin", user.Rows[0]["isAdmin"].ToString()!);

            return Redirect("/");
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}