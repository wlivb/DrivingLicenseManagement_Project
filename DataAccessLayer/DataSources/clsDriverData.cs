using System;
using System.Data;
using DVLD_DTOs;

namespace DataAccessLayer.DataSources
{
    public class clsDriverData
    {
        public static DriverDTO GetDriverInfoByID(int driverId)
        {
            DriverDTO driver = null;
            string query = "SELECT * FROM Drivers WHERE DriverID = @ID";

            clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", driverId);
                },
                reader =>
                {
                    driver = new DriverDTO
                    {
                        DriverID = (int)reader["DriverID"],
                        PersonID = (int)reader["PersonID"],
                        CreatedByUserID = (int)reader["CreatedByUserID"],
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    };
                });
            return driver;
        }
        public static DriverDTO GetDriverInfoByPersonID(int personId)
        {
            DriverDTO driver = null;
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

            clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@PersonID", personId);
                },
                reader =>
                {
                    driver = new DriverDTO
                    {
                        DriverID = (int)reader["DriverID"],
                        PersonID = (int)reader["PersonID"],
                        CreatedByUserID = (int)reader["CreatedByUserID"],
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    };
                });
            return driver;
        }
        public static DataTable GetAllDrivers()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM v_DriversList order by FullName");
        }
        public static int AddNewDriver(DriverDTO driver)
        {
            string query = "INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate) " +
                           "VALUES (@PersonID, @CreatedByUserID, @CreatedDate); " +
                           "SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query,
                   cmd =>
                   {
                       cmd.Parameters.AddWithValue("@PersonID", driver.PersonID);
                       cmd.Parameters.AddWithValue("@CreatedByUserID", driver.CreatedByUserID);
                       cmd.Parameters.AddWithValue("@CreatedDate", driver.CreatedDate);
                   });
        }
        public static bool UpdateDriver(DriverDTO driver)
        {
            string query = "UPDATE Drivers SET PersonID = @PersonID, CreatedByUserID = @CreatedByUserID, CreatedDate = @CreatedDate " +
                           "WHERE DriverID = @DriverID";

            int rowsAffected = clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@PersonID", driver.PersonID);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", driver.CreatedByUserID);
                    cmd.Parameters.AddWithValue("@CreatedDate", driver.CreatedDate);
                    cmd.Parameters.AddWithValue("@DriverID", driver.DriverID);
                });
            return rowsAffected > 0;
        }
    }
}
