using System;
using System.Data;
using DVLD_DTOs;

namespace DataAccessLayer.DataSources
{
    public class clsLicenseData
    {
        public static LicenseDTO GetLicenseInfoByID(int licenseId)
        {
            LicenseDTO license = null;

            string query = "SELECT * FROM Licenses WHERE LicenseID = @ID";

            clsSqlHelper.ExecuteReader(query,
                        cmd =>
                        {
                            cmd.Parameters.AddWithValue("@ID", licenseId);
                        },
                        reader =>
                        {
                            license = new LicenseDTO
                            {
                                LicenseID = (int)reader["LicenseID"],
                                ApplicationID = (int)reader["ApplicationID"],
                                DriverID = (int)reader["DriverID"],
                                LicenseClassID = (int)reader["LicenseClassID"],
                                IssueDate = (DateTime)reader["IssueDate"],
                                ExpirationDate = (DateTime)reader["ExpirationDate"],
                                Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : string.Empty,
                                PaidFees = (decimal)reader["PaidFees"],
                                IsActive = (bool)reader["IsActive"],
                                IssueReason = (byte)reader["IssueReason"],
                                CreatedByUserID = (int)reader["CreatedByUserID"]
                            };
                        });
            return license;
        }
        public static DataTable GetAllLicenses()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM Licenses");
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            string query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClassID ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc";

            return clsSqlHelper.ExecuteDataTable(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                });
        }
        public static int AddNewLicense(LicenseDTO license)
        {
            string query = @"INSERT INTO Licenses (ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                             VALUES (@ApplicationID, @DriverID, @LicenseClassID, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", license.ApplicationID);
                    cmd.Parameters.AddWithValue("@DriverID", license.DriverID);
                    cmd.Parameters.AddWithValue("@LicenseClassID", license.LicenseClassID);
                    cmd.Parameters.AddWithValue("@IssueDate", license.IssueDate);
                    cmd.Parameters.AddWithValue("@ExpirationDate", license.ExpirationDate);
                    cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(license.Notes) ? (object)DBNull.Value : license.Notes);
                    cmd.Parameters.AddWithValue("@PaidFees", license.PaidFees);
                    cmd.Parameters.AddWithValue("@IsActive", license.IsActive);
                    cmd.Parameters.AddWithValue("@IssueReason", license.IssueReason);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", license.CreatedByUserID);
                });
        }
        public static bool UpdateLicense(LicenseDTO license)
        {
            string query = @"UPDATE Licenses 
                            SET ApplicationID = @ApplicationID,     
                            DriverID = @DriverID,                   
                            LicenseClassID = @LicenseClassID,       
                            IssueDate = @IssueDate,                 
                            ExpirationDate = @ExpirationDate,       
                            Notes = @Notes,                         
                            PaidFees = @PaidFees,                   
                            IsActive = @IsActive,                   
                            IssueReason = @IssueReason,             
                            CreatedByUserID = @CreatedByUserID      
                            WHERE LicenseID = @LicenseID";

            return clsSqlHelper.ExecuteNonQuery(query,
                                cmd =>
                                {
                                    cmd.Parameters.AddWithValue("@LicenseID", license.LicenseID);
                                    cmd.Parameters.AddWithValue("@ApplicationID", license.ApplicationID);
                                    cmd.Parameters.AddWithValue("@DriverID", license.DriverID);
                                    cmd.Parameters.AddWithValue("@LicenseClassID", license.LicenseClassID);
                                    cmd.Parameters.AddWithValue("@IssueDate", license.IssueDate);
                                    cmd.Parameters.AddWithValue("@ExpirationDate", license.ExpirationDate);
                                    cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(license.Notes) ? (object)DBNull.Value : license.Notes); cmd.Parameters.AddWithValue("@PaidFees", license.PaidFees);
                                    cmd.Parameters.AddWithValue("@IsActive", license.IsActive);
                                    cmd.Parameters.AddWithValue("@IssueReason", license.IssueReason);
                                    cmd.Parameters.AddWithValue("@CreatedByUserID", license.CreatedByUserID);
                                }) > 0;

        }
        public static int GetActiveLicenseIDByPersonID(int personID, int licenseClassID)
        {
            string query = @"SELECT TOP 1 Licenses.LicenseID
                           FROM Licenses 
                           INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID
                           WHERE Drivers.PersonID = @PersonID 
                           AND Licenses.LicenseClassID = @LicenseClassId 
                           AND Licenses.IsActive = 1
                           ORDER BY Licenses.LicenseID DESC;";

            return clsSqlHelper.ExecuteScalar(query,
                                cmd =>
                                {
                                       cmd.Parameters.AddWithValue("@PersonID", personID);
                                       cmd.Parameters.AddWithValue("@LicenseClassId", licenseClassID);
                                });
        }
        public static bool DeactivateLicense(int licenseID)
        {
            string query = @"UPDATE Licenses SET IsActive = 0 WHERE LicenseID = @LicenseID";

            return clsSqlHelper.ExecuteNonQuery(query,
                                cmd =>
                                {
                                    cmd.Parameters.AddWithValue("@LicenseID", licenseID);
                                }) > 0;
        }
    }
}
