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
        public List<LeaderboardEntryMO> LeaderboardEntriesMO { get; set; } = new List<LeaderboardEntryMO>();

        public LeaderBoard_MonacoModel(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void OnGet()
        {
            string query = @"
                SELECT TOP 10
                    ROW_NUMBER() OVER (ORDER BY p.MonacoRecord ASC) AS Rank,
                    u.DisplayName AS PlayerName,
                    p.MonacoRecord AS LapTime
                FROM Performance p
                INNER JOIN Users u ON p.UserId = u.UserId
                WHERE p.MonacoRecord IS NOT NULL
                ORDER BY p.MonacoRecord ASC
            ";

            DataTable dt = _dbHelper.GetData(query);
            LeaderboardEntriesMO = new List<LeaderboardEntryMO>();

            foreach (DataRow row in dt.Rows)
            {
                // Only add if LapTime is not null
                if (row["LapTime"] != DBNull.Value)
                {
                    LeaderboardEntriesMO.Add(new LeaderboardEntryMO
                    {
                        Rank = Convert.ToInt32(row["Rank"]),
                        UserName = row["PlayerName"].ToString()!,
                        LapTime = (TimeSpan)row["LapTime"]
                    });
                }
            }
        }

        public IActionResult OnPostSaveLap([FromBody] LapTimeRequestMO request)
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
                        INSERT INTO Performance (UserId, MonacoRecord, LastRaceDate, TotalRaces)
                        VALUES (@UserId, @LapTime, GETDATE(), 1)
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
                        SET MonacoRecord = @LapTime,
                            LastRaceDate = GETDATE(),
                            TotalRaces = TotalRaces + 1
                        WHERE UserId = @UserId
                        AND (MonacoRecord IS NULL OR @LapTime < MonacoRecord)
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
    public class LeaderboardEntryMO
    {
        public int Rank { get; set; }
        public string UserName { get; set; } = "";
        public TimeSpan LapTime { get; set; }
    }
    public class LapTimeRequestMO
    {
        public double LapTimeSeconds { get; set; }
    }
}
