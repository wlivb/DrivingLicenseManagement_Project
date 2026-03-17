using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public static class clsSqlHelper
    {
        public static bool ExecuteReader(string query, Action<SqlCommand> setParams, Action<SqlDataReader> readRow)
        {
            bool isFound = false;
      
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    setParams(command); 
      
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                readRow(reader); 
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("SQL Error: " + ex.Message);
                        isFound = false; 
                    }
                }
            }
            return isFound;
        }
        public static int ExecuteScalar(string query, Action<SqlCommand> setParams)
        {
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                setParams(cmd);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return (result != null && int.TryParse(result.ToString(), out int id)) ? id : -1;
            }
        }
        public static int ExecuteNonQuery(string query, Action<SqlCommand> setParams)
        {
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                setParams(cmd);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        public static DataTable ExecuteDataTable(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) dt.Load(reader);
                    }
                }
            }
            catch 
            {
                /* Log Error */
            }
            return dt;
        }
        public static DataTable ExecuteDataTable(string query, Action<SqlCommand> setParams)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    setParams(cmd);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        dt.Load(reader);
                    }
                }
            }
            catch
            {
                /* Log Error */
            }
            return dt;
        }
        public static bool IsExist(string query, Action<SqlCommand> setParams)
        {
            bool isFound = false;
            using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                setParams(cmd);
                conn.Open();
                object result = cmd.ExecuteScalar();
                isFound = (result != null);
            }
            return isFound;
        }
    }
}
