using System;
using System.Data;
using DVLD_DTOs;

namespace DataAccessLayer.DataSources
{
    public class clsDetainedLicenseData
    {
        public static DetainedLicenseDTO GetDetainedLicenseInfoByID(int detainedLicenseID)
        {
            DetainedLicenseDTO dto = null;

            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @ID";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@ID", detainedLicenseID);
                         },
                         reader =>
                         {
                             dto = new DetainedLicenseDTO
                             {
                                 DetainID = (int)reader["DetainID"],
                                 LicenseID = (int)reader["LicenseID"],
                                 DetainDate = (DateTime)reader["DetainDate"],
                                 FineFees = (decimal)reader["FineFees"],
                                 CreatedByUserID = (int)reader["CreatedByUserID"],
                                 IsReleased = (bool)reader["IsReleased"],
                                 ReleaseDate = reader["ReleaseDate"] != DBNull.Value ? (DateTime)reader["ReleaseDate"] : DateTime.MinValue,
                                 ReleasedByUserID = reader["ReleasedByUserID"] != DBNull.Value ? (int)reader["ReleasedByUserID"] : -1,
                                 ReleaseApplicationID = reader["ReleaseApplicationID"] != DBNull.Value ? (int)reader["ReleaseApplicationID"] : -1
                             };
                         });
            return dto;
        }
        public static DetainedLicenseDTO GetDetainedLicenseInfoByLicenseID(int licenseID)
        {
            DetainedLicenseDTO dto = null;

            string query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @ID";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@ID", licenseID);
                         },
                         reader =>
                         {
                             dto = new DetainedLicenseDTO
                             {
                                 DetainID = (int)reader["DetainID"],
                                 LicenseID = (int)reader["LicenseID"],
                                 DetainDate = (DateTime)reader["DetainDate"],
                                 FineFees = (decimal)reader["FineFees"],
                                 CreatedByUserID = (int)reader["CreatedByUserID"],
                                 IsReleased = (bool)reader["IsReleased"],
                                 ReleaseDate = reader["ReleaseDate"] != DBNull.Value ? (DateTime)reader["ReleaseDate"] : DateTime.MinValue,
                                 ReleasedByUserID = reader["ReleasedByUserID"] != DBNull.Value ? (int)reader["ReleasedByUserID"] : -1,
                                 ReleaseApplicationID = reader["ReleaseApplicationID"] != DBNull.Value ? (int)reader["ReleaseApplicationID"] : -1
                             };
                         });
            return dto;
        }
        public static DataTable GetAllDetainedLicenses()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM v_DetainedLicensesList WHERE IsReleased = 0");
        }
        public static int AddNew(DetainedLicenseDTO dto)
        {
            string query = @"INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased) 
                             VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased);
                             SELECT SCOPE_IDENTITY();";
            return clsSqlHelper.ExecuteScalar(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@LicenseID", dto.LicenseID);
                             cmd.Parameters.AddWithValue("@DetainDate", dto.DetainDate);
                             cmd.Parameters.AddWithValue("@FineFees", dto.FineFees);
                             cmd.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                             cmd.Parameters.AddWithValue("@IsReleased", dto.IsReleased);
                         });
        }
        public static bool Update(DetainedLicenseDTO dto)
        {
            string query = @"UPDATE DetainedLicenses 
                             SET LicenseID = @LicenseID, DetainDate = @DetainDate, FineFees = @FineFees, CreatedByUserID = @CreatedByUserID, IsReleased = @IsReleased, 
                                 ReleaseDate = @ReleaseDate, ReleasedByUserID = @ReleasedByUserID, ReleaseApplicationID = @ReleaseApplicationID
                             WHERE DetainID = @DetainID";

            return clsSqlHelper.ExecuteNonQuery(query,
                                cmd =>
                                {
                                    cmd.Parameters.AddWithValue("@LicenseID", dto.LicenseID);
                                    cmd.Parameters.AddWithValue("@DetainDate", dto.DetainDate);
                                    cmd.Parameters.AddWithValue("@FineFees", dto.FineFees);
                                    cmd.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                                    cmd.Parameters.AddWithValue("@IsReleased", dto.IsReleased);
                                    cmd.Parameters.AddWithValue("@ReleaseDate", dto.ReleaseDate != DateTime.MinValue ? (object)dto.ReleaseDate : DBNull.Value);
                                    cmd.Parameters.AddWithValue("@ReleasedByUserID", dto.ReleasedByUserID != -1 ? (object)dto.ReleasedByUserID : DBNull.Value);
                                    cmd.Parameters.AddWithValue("@ReleaseApplicationID", dto.ReleaseApplicationID != -1 ? (object)dto.ReleaseApplicationID : DBNull.Value);
                                    cmd.Parameters.AddWithValue("@DetainID", dto.DetainID);

                                }) > 0;
        }
        public static bool ReleaseDetainedLicense(int detainID, int releasedByUserID, int releaseApplicationID)
        {
            string query = @"UPDATE DetainedLicenses 
                             SET IsReleased = 1, ReleaseDate = GETDATE(), ReleasedByUserID = @ReleasedByUserID, ReleaseApplicationID = @ReleaseApplicationID
                             WHERE DetainID = @DetainID";

            return clsSqlHelper.ExecuteNonQuery(query,
                                cmd =>
                                {
                                    cmd.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserID);
                                    cmd.Parameters.AddWithValue("@ReleaseApplicationID", releaseApplicationID);
                                    cmd.Parameters.AddWithValue("@DetainID", detainID);
                                }) > 0;
        }
        public static bool IsLicenseDetained(int licenseID)
        {
            string query = "SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @ID ORDER BY DetainID DESC";

            return clsSqlHelper.ExecuteScalar(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@ID", licenseID);

                         }) > 0;
        }
    }
}
