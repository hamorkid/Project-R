using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Project_R_._8.Services;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Project_R_._8.Pages.cshtml
{
    public class SignUpModel : PageModel
    {
        private readonly DBHelper _db;

        public SignUpModel(DBHelper db)
        {
            _db = db;
        }

        // Form fields
        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Display { get; set; } = "";
        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";
        [BindProperty] public string RePassword { get; set; } = "";
        [BindProperty] public string Phone { get; set; } = "";
        [BindProperty] public string gender { get; set; } = "";
        [BindProperty] public int city { get; set; }
        [BindProperty] public int q1 { get; set; }
        [BindProperty] public string answer1 { get; set; } = "";
        [BindProperty] public int q2 { get; set; }
        [BindProperty] public string answer2 { get; set; } = "";

        // Dropdowns
        public List<SelectListItem> Cities { get; set; } = new();
        public List<SelectListItem> SecurityQuestions { get; set; } = new();

        public void OnGet()
        {
            var testConn = HttpContext.RequestServices.GetService<IConfiguration>()
            ?.GetConnectionString("DefaultConnection");
            Console.WriteLine("TEST CONNECTION STRING: " + testConn);
            LoadDropdowns();
        }

        public IActionResult OnPost()
        {
            LoadDropdowns();

            if (Password != RePassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return Page();
            }

            // Hash the password
            string passwordHash = HashPassword(Password);

            // Check if email already exists
            DataTable existing = _db.GetData($"SELECT * FROM Users WHERE Email = '{Email}'");
            if (existing.Rows.Count > 0)
            {
                ModelState.AddModelError("", "Email already registered");
                return Page();
            }

            bool isMale = gender == "man";

            _db.ExecuteQuery(
                @"INSERT INTO Users 
                (UserName, Email, PasswordHash, DisplayName, Gender, CityId, 
                SecurityQuestion1Id, SecurityAnswer1, SecurityQuestion2Id, SecurityAnswer2, Phone)
                VALUES 
                (@Username, @Email, @PasswordHash, @DisplayName, @Gender, @CityId,
                @Q1, @A1, @Q2, @A2, @Phone)",
                new SqlParameter[]
                {
                    new SqlParameter("@Username", Username),
                    new SqlParameter("@Email", Email),
                    new SqlParameter("@PasswordHash", passwordHash),
                    new SqlParameter("@DisplayName", Display),
                    new SqlParameter("@Gender", isMale),
                    new SqlParameter("@CityId", city),
                    new SqlParameter("@Q1", q1),
                    new SqlParameter("@A1", answer1),
                    new SqlParameter("@Q2", q2),
                    new SqlParameter("@A2", answer2),
                    new SqlParameter("@Phone", Phone)
                }
            );

            return RedirectToPage("/cshtml/Login");
        }

        private void LoadDropdowns()
        {
            // Load cities from DB
            DataTable citiesTable = _db.GetData("SELECT CityId, CityName FROM Cities");
            Cities = citiesTable.AsEnumerable().Select(row => new SelectListItem
            {
                Value = row["CityId"].ToString(),
                Text = row["CityName"].ToString()
            }).ToList();

            // Load security questions from DB
            DataTable questionsTable = _db.GetData("SELECT SecurityQuestionId, Question FROM SecurityQuestions");
            SecurityQuestions = questionsTable.AsEnumerable().Select(row => new SelectListItem
            {
                Value = row["SecurityQuestionId"].ToString(),
                Text = row["Question"].ToString()
            }).ToList();
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}