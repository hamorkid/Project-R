using Microsoft.Data.SqlClient;
using System.Data;

namespace Project_R_._8.Services
{
    public class DBHelper
    {
        private readonly string connStr;
        public DBHelper(IConfiguration configuration)
        {
            string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "APP_DATA", "Accounts.mdf");
            Console.WriteLine($"Database path: {dbPath}"); // Debug line
            connStr = $"Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
        }

        public DataTable GetData(string query, SqlParameter[]? parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters); // Add this line!

                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Database Error: {ex.Message}"); throw; }
            return dt;
        }

        public int ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}