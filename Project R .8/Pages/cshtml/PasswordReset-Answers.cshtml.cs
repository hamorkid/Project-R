using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_R_._8.Services;
using System.Data;

namespace Project_R_._8.Pages.cshtml
{
    public class PasswordReset_AnswersModel : PageModel
    {
        private readonly DBHelper _db;

        public PasswordReset_AnswersModel(DBHelper db)
        {
            _db = db;
        }

        public string ErrorMessage { get; set; } = "";
        public string q1 { get; set; } = "";
        public string q2 { get; set; } = "";

        [BindProperty] public string A1 { get; set; } = "";
        [BindProperty] public string A2 { get; set; } = "";

        public IActionResult OnGet()
        {
            if (TempData.Peek("Username") == null)
            {
                return RedirectToPage("/cshtml/PasswordReset-Username");
            }

            string username = TempData.Peek("Username")!.ToString()!;

            DataTable user = _db.GetData($@"
                SELECT u.SecurityAnswer1, u.SecurityAnswer2,
                       q1.Question AS Question1, q2.Question AS Question2
                FROM Users u
                JOIN SecurityQuestions q1 ON u.SecurityQuestion1Id = q1.SecurityQuestionId
                JOIN SecurityQuestions q2 ON u.SecurityQuestion2Id = q2.SecurityQuestionId
                WHERE u.UserName = '{username}'
            ");

            if (user.Rows.Count > 0)
            {
                q1 = user.Rows[0]["Question1"].ToString()!;
                q2 = user.Rows[0]["Question2"].ToString()!;

                // Store questions in TempData so OnPost can access them
                TempData["Q1"] = q1;
                TempData["Q2"] = q2;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(A1) || string.IsNullOrWhiteSpace(A2))
            {
                q1 = TempData.Peek("Q1")?.ToString() ?? "";
                q2 = TempData.Peek("Q2")?.ToString() ?? "";
                ErrorMessage = "Please answer both questions";
                return Page();
            }

            string username = TempData.Peek("Username")!.ToString()!;

            // Restore questions for display if validation fails
            q1 = TempData.Peek("Q1")?.ToString() ?? "";
            q2 = TempData.Peek("Q2")?.ToString() ?? "";

            DataTable user = _db.GetData($@"
                SELECT SecurityAnswer1, SecurityAnswer2 
                FROM Users WHERE UserName = '{username}'
            ");

            if (user.Rows.Count == 0)
            {
                ErrorMessage = "User not found";
                return Page();
            }

            string correctA1 = user.Rows[0]["SecurityAnswer1"].ToString()!;
            string correctA2 = user.Rows[0]["SecurityAnswer2"].ToString()!;

            if (A1.Trim().ToLower() != correctA1.Trim().ToLower() ||
                A2.Trim().ToLower() != correctA2.Trim().ToLower())
            {
                ErrorMessage = "Please recheck your answers";
                return Page();
            }

            return RedirectToPage("/cshtml/PasswordReset-CreatePassword");
        }
    }
}