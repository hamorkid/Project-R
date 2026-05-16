using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_R_._8.Services;
using Microsoft.Data.SqlClient;
using System.Data;
namespace Project_R_._8.Pages.cshtml
{
    public class LeaderBoard_MonacoModel : PageModel
    {
        private readonly DBHelper _dbHelper;
        public List<LeaderboardEntry> LeaderboardEntriesMO { get; set; }

        public LeaderBoard_MonacoModel(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void OnGet()
        {
            string query = @"
                SELECT TOP 10
                    ROW_NUMBER() OVER (ORDER BY p.LasVegasRecord ASC) AS Rank,
                    u.DisplayName AS PlayerName,
                    p.LasVegasRecord AS LapTime
                FROM Performance p
                INNER JOIN Users u ON p.UserId = u.UserId
                WHERE p.LasVegasRecord IS NOT NULL
                ORDER BY p.LasVegasRecord ASC
            ";

            DataTable dt = _dbHelper.GetData(query);
            LeaderboardEntriesMO = new List<LeaderboardEntry>();

            foreach (DataRow row in dt.Rows)
            {
                LeaderboardEntriesMO.Add(new LeaderboardEntry
                {
                    Rank = Convert.ToInt32(row["Rank"]),
                    UserName = row["PlayerName"].ToString(),
                    LapTime = (TimeSpan)row["LapTime"]
                });
            }
        }

        public IActionResult OnPostSaveLap([FromBody] LapTimeRequest request)
        {
            Console.WriteLine("OnPostSaveLap called"); // Debug line
            var userIdString = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"UserId from session: {userIdString}"); // Debug line
            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest(new { message = "User not authenticated" });
            }

            try
            {
                // Convert seconds to TIME format (HH:MM:SS.mmm)
                Console.WriteLine($"Saving lap time: {request.LapTimeSeconds}"); // Debug line
                TimeSpan lapTime = TimeSpan.FromSeconds(request.LapTimeSeconds);

                // Check if user has a performance record
                string checkQuery = "SELECT COUNT(*) FROM Performance WHERE UserId = @UserId";
                DataTable checkDt = _dbHelper.GetData(checkQuery, new[] { new SqlParameter("@UserId", userId) });

                int count = (int)checkDt.Rows[0][0];

                if (count == 0)
                {
                    // Insert new record
                    string insertQuery = @"
                        INSERT INTO Performance (UserId, LasVegasRecord, scorePoints, LastRaceDate, TotalRaces)
                        VALUES (@UserId, @LapTime, 0, GETDATE(), 1)
                    ";
                    _dbHelper.ExecuteQuery(insertQuery, new[]
                    {
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@LapTime", lapTime)
                    });
                }
                else
                {
                    // Update existing record if new time is better
                    string updateQuery = @"
                        UPDATE Performance
                        SET LasVegasRecord = @LapTime,
                            LastRaceDate = GETDATE(),
                            TotalRaces = TotalRaces + 1
                        WHERE UserId = @UserId
                        AND (LasVegasRecord IS NULL OR @LapTime < LasVegasRecord)
                    ";
                    _dbHelper.ExecuteQuery(updateQuery, new[]
                    {
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@LapTime", lapTime)
                    });
                }

                return new JsonResult(new { message = "Lap time saved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error saving lap time: {ex.Message}" });
            }
        }
    }
}
