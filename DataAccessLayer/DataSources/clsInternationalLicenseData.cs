using System;
using System.Data;
using DVLD_DTOs;

namespace DataAccessLayer.DataSources
{
    public class clsInternationalLicenseData
    {
        public static InternationalLicenseDTO GetInternationalLicenseInfoByID(int internationalLicenseID)
        {
            InternationalLicenseDTO international = null;

            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @ID";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@ID", internationalLicenseID);
                         },
                         reader =>
                         {
                             international = new InternationalLicenseDTO
                             {
                                 InternationalLicenseID = (int)reader["InternationalLicenseID"],
                                 ApplicationID = (int)reader["ApplicationID"],
                                 DriverID = (int)reader["DriverID"],
                                 IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
                                 IssueDate = (DateTime)reader["IssueDate"],
                                 ExpirationDate = (DateTime)reader["ExpirationDate"],
                                 IsActive = (bool)reader["IsActive"],
                                 CreatedByUserID = (int)reader["CreatedByUserID"]
                             };

                         });

            return international;
        }
        public static DataTable GetAllInternationalLicenses()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM InternationalLicenses order by IsActive, ExpirationDate desc");
        }
        public static DataTable GetDriverInternationalLicenses(int driverID)
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM InternationalLicenses WHERE DriverID = @DriverID order by ExpirationDate desc",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@DriverID", driverID);
                });
        }
        public static int AddNew(InternationalLicenseDTO dto)
        {
            string query = @"Update InternationalLicenses set IsActive=0 where DriverID=@ID;               
                             INSERT INTO InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID)                      
                             VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);                      
                             SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query,
                                cmd =>
                                {
                                    cmd.Parameters.AddWithValue("@ID", dto.DriverID);
                                    cmd.Parameters.AddWithValue("@ApplicationID", dto.ApplicationID);
                                    cmd.Parameters.AddWithValue("@DriverID", dto.DriverID);
                                    cmd.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", dto.IssuedUsingLocalLicenseID);
                                    cmd.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                                    cmd.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
                                    cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);
                                    cmd.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                                });
        }
        public static bool Update(InternationalLicenseDTO dto)
        {
            string query = @"UPDATE InternationalLicenses
                         SET ApplicationID = @ApplicationID,
                             DriverID = @DriverID,
                             IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                             IssueDate = @IssueDate,
                             ExpirationDate = @ExpirationDate,
                             IsActive = @IsActive
                         WHERE InternationalLicenseID = @InternationalLicenseID";

            return clsSqlHelper.ExecuteNonQuery(query,
                                cmd =>
                                {
                                    cmd.Parameters.AddWithValue("@ApplicationID", dto.ApplicationID);
                                    cmd.Parameters.AddWithValue("@DriverID", dto.DriverID);
                                    cmd.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", dto.IssuedUsingLocalLicenseID);
                                    cmd.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                                    cmd.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
                                    cmd.Parameters.AddWithValue("@IsActive", dto.IsActive);
                                    cmd.Parameters.AddWithValue("@InternationalLicenseID", dto.InternationalLicenseID);
                                }) > 0;
        }
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            string query = @"SELECT Top 1 InternationalLicenseID
                  FROM InternationalLicenses 
                  where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                  order by ExpirationDate Desc;";

            return clsSqlHelper.ExecuteScalar(query,
                                cmd =>
                                {
                                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                                }); 
        }
        public static InternationalLicenseFullInfoDTO GetInternationalLicenseFullInfoByID(int ID)
        {
            InternationalLicenseFullInfoDTO fullInfo = null;

            string query = @"SELECT * FROM InternationalLicenses 
                     INNER JOIN Applications ON InternationalLicenses.ApplicationID = Applications.ApplicationID
                     WHERE InternationalLicenseID = @ID";

            clsSqlHelper.ExecuteReader(query,
                cmd => cmd.Parameters.AddWithValue("@ID", ID),
                reader =>
                {
                    fullInfo = new InternationalLicenseFullInfoDTO
                    {
                        Application = new ApplicationDTO(
                            (int)reader["ApplicationID"], (int)reader["ApplicantPersonID"],
                            (DateTime)reader["ApplicationDate"], (int)reader["ApplicationTypeID"],
                            (byte)reader["ApplicationStatus"], (DateTime)reader["LastStatusDate"],
                            (decimal)reader["PaidFees"], (int)reader["CreatedByUserID"]
                        ),
                        InternationalLicense = new InternationalLicenseDTO(
                                   (int)reader["InternationalLicenseID"],
                                   (int)reader["ApplicationID"],
                                   (int)reader["DriverID"],
                                   (int)reader["IssuedUsingLocalLicenseID"],
                                   (DateTime)reader["IssueDate"],
                                   (DateTime)reader["ExpirationDate"],
                                   (bool)reader["IsActive"],
                                   (int)reader["CreatedByUserID"]
                        )
                    };
                });

            return fullInfo;
        }
    }
}
