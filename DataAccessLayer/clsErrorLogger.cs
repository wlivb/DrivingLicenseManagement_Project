using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.clsSqlHelper;

namespace DataAccessLayer
{
    public static class clsErrorLogger
    {
        public static void Subscribe()
        {
            clsSqlHelper.OnErrorOccurred += HandleError;
        }
        private static void HandleError(object sender, ErrorLogEventArgs e)
        {
            if (!TryLogToDatabase(e))
            {
                LogToFile(e);
            }
        }
        private static bool TryLogToDatabase(ErrorLogEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"INSERT INTO ErrorLogs (Message, StackTrace, QueryText) 
                                     VALUES (@msg, @stack, @query)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@msg", e.Message);
                        cmd.Parameters.AddWithValue("@stack", (object)e.StackTrace ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@query", (object)e.QueryText ?? DBNull.Value);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }            
                }
            }
            catch 
            {
                return false;
            } 
        }
        private static void LogToFile(ErrorLogEventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorsLog.txt");
            string logEntry = $"\r\nLog [{DateTime.Now}]:\nMsg: {e.Message}\nQuery: {e.QueryText}\n{new string('-', 30)}";

            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(logEntry);
            } 
        }
    }
}
