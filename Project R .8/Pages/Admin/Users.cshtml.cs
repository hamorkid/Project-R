using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Project_R_._8.Services;
using System.Data;

namespace Project_R_._8.Pages.Admin
{
    public class ManageUsersModel : PageModel
    {
        private readonly DBHelper _dbHelper;
        public ManageUsersModel(DBHelper dbHelper) => _dbHelper = dbHelper!;

        public List<UserUpdateModel> UsersList { get; set; } = new List<UserUpdateModel>();

        [BindProperty] public UserUpdateModel EditUser { get; set; } = new();

        public IActionResult OnGet()
        {

            string? adminStatus = HttpContext.Session.GetString("IsAdmin")?.ToLower();

            // 1. If it's null, they aren't logged in
            if (adminStatus == null)
            {
                return RedirectToPage("/cshtml/Login");
            }

            // 2. If it's not "true", they aren't an admin
            if (adminStatus != "true")
            {
                return RedirectToPage("/cshtml/HomePage");
            }



            string query = "SELECT UserId, UserName, Email, DisplayName, Phone, CityId, Gender, isAdmin FROM Users";
            DataTable dt = _dbHelper.GetData(query);

            foreach (DataRow row in dt.Rows)
            {
                UsersList.Add(new UserUpdateModel
                {
                    UserId = (int)row["UserId"],
                    UserName = row["UserName"]?.ToString() ?? string.Empty,
                    Email = row["Email"]?.ToString() ?? string.Empty,
                    DisplayName = row["DisplayName"]?.ToString() ?? string.Empty,
                    Phone = row["Phone"]?.ToString() ?? string.Empty,
                    CityId = (int)row["CityId"],
                    Gender = (bool)row["Gender"],
                    IsAdmin = (bool)row["isAdmin"]
                });
            }

            return Page();
        }

        public IActionResult OnPostUpdateUser()
        {
            string query = @"UPDATE Users SET UserName=@UN, Email=@Email, DisplayName=@DN, 
                             Phone=@Phone, CityId=@City, Gender=@Gender, isAdmin=@Admin 
                             WHERE UserId=@ID";

            SqlParameter[] ps = {
                new SqlParameter("@UN", EditUser.UserName),
                new SqlParameter("@Email", EditUser.Email),
                new SqlParameter("@DN", EditUser.DisplayName),
                new SqlParameter("@Phone", EditUser.Phone),
                new SqlParameter("@City", EditUser.CityId),
                new SqlParameter("@Gender", EditUser.Gender),
                new SqlParameter("@Admin", EditUser.IsAdmin),
                new SqlParameter("@ID", EditUser.UserId)
            };

            _dbHelper.ExecuteQuery(query, ps);
            return RedirectToPage();
        }
    }

    public class UserUpdateModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int CityId { get; set; }
        public bool Gender { get; set; }
        public bool IsAdmin { get; set; }
    }
}