using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public static class clsSqlHelper
    {
        public static bool ExecuteReader(string query, Action<SqlCommand> setParams, Action<SqlDataReader> readRow)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    setParams?.Invoke(command);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            readRow(reader);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                clsLogger.LogError("SQL Error: " + ex.Message + "\nQuery: " + query);
            }
            return false;
        }
        public static int ExecuteScalar(string query, Action<SqlCommand> setParams)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    setParams?.Invoke(cmd);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return (result != null && int.TryParse(result.ToString(), out int id)) ? id : -1;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError("SQL Error: " + ex.Message + "\nQuery: " + query);
                return -1;
            }
        }
        public static int ExecuteNonQuery(string query, Action<SqlCommand> setParams)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    setParams?.Invoke(cmd);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) 
            {
                clsLogger.LogError("SQL Error: " + ex.Message + "\nQuery: " + query);
                return -1;
            }
        }
        public static DataTable ExecuteDataTable(string query, Action<SqlCommand> setParams = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    setParams?.Invoke(cmd);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex) 
            {
                clsLogger.LogError("SQL Error: " + ex.Message + "\nQuery: " + query);
            }
            return dt;
        }
        public static bool IsExist(string query, Action<SqlCommand> setParams)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    setParams?.Invoke(cmd);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogError("SQL Error: " + ex.Message + "\nQuery: " + query);
            }
            return false;
        }
    }
}
