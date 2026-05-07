using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_R_._8.Pages.cshtml
{
    public class PasswordReset_AnswersModel : PageModel
    {
        public string ErrorMessage { get; set; } = "";
        public string Q1 { get; set; } = "";
        public string Q2 { get; set; } = "";
        [BindProperty] public string A1 { get; set; } = "";
        [BindProperty] public string A2 { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        private readonly DBHelper _db;
        private readonly PasswordReset_UsernameModel UsernameModel;


        public PasswordReset_AnswersModel(DBHelper db)
        {
            _db = db;
        }

        public void OnGet()
        {
            DataTable user = _db.GetData(
                $"SELECT * FROM Users WHERE UserName = '{UsernameModel.SendUsername()}'"
            );
            
            Q1 = user.Rows[0]["Q1"].ToString()!;
            Q2 = user.Rows[0]["Q2"].ToString()!;
        }

        public IActionResult OnPost()
        {
            DataTable user = _db.GetData(
                $"SELECT * FROM Users WHERE UserName = '{UsernameModel.SendUsername()}'"
            );

            if (!((A1 == user.Rows[0]["A1"].ToString()!) && (A2 == user.Rows[0]["A2"].ToString()!)))
            {
                ErrorMessage = "Please recheck your answers";
                return Page();
            }
            return RedirectToPage("/cshtml/PasswordReset-Answers"); 
        }
    }
}
