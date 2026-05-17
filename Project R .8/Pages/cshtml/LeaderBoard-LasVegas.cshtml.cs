using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_R_._8.Services;
using Microsoft.Data.SqlClient;
using System.Data;
namespace Project_R_._8.Pages.cshtml
{
    public class LeaderBoard_LasVegasModel : PageModel
    {
        private readonly DBHelper _dbHelper;
        public List<LeaderboardEntryLV> LeaderboardEntriesLV { get; set; } = new List<LeaderboardEntryLV>();

        public LeaderBoard_LasVegasModel(DBHelper dbHelper)
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
            LeaderboardEntriesLV = new List<LeaderboardEntryLV>();

            foreach (DataRow row in dt.Rows)
            {
                // Only add if LapTime is not null
                if (row["LapTime"] != DBNull.Value)
                {
                    LeaderboardEntriesLV.Add(new LeaderboardEntryLV
                    {
                        Rank = Convert.ToInt32(row["Rank"]),
                        UserName = row["PlayerName"].ToString()!,
                        LapTime = (TimeSpan)row["LapTime"]
                    });
                }
            }
        }

        public IActionResult OnPostSaveLap([FromBody] LapTimeRequestLV request)
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
                        INSERT INTO Performance (UserId, LasVegasRecord, LastRaceDate, TotalRaces)
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

    public class LeaderboardEntryLV
    {
        public int Rank { get; set; }
        public string UserName { get; set; } = "";
        public TimeSpan LapTime { get; set; }
    }

    public class LapTimeRequestLV
    {
        public double LapTimeSeconds { get; set; }
    }
}
