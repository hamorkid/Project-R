using Microsoft.Data.SqlClient;
using System.Data;

namespace Project_R_._8.Services
{
    public class DBHelper
    {
        private readonly string connStr;

        public DBHelper(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("DefaultConnection")!;
        }

        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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