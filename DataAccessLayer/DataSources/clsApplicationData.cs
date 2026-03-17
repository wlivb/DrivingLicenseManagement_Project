using DVLD_DTOs;
using System;
using System.Data;

namespace DataAccessLayer.DataSources
{
    public class clsApplicationData
    {

        public static ApplicationDTO GetApplicationInfoByID(int applicationID)
        {
            ApplicationDTO Application = null;

            string query = "Select * From Applications Where ApplicationID = @ID";

            clsSqlHelper.ExecuteReader(query,
             cmd =>
             {
                 cmd.Parameters.AddWithValue("@ID", applicationID);
             },
             reader =>
             {
                 Application = new ApplicationDTO
                 {
                     ApplicationID = (int)reader["ApplicationID"],
                     ApplicantPersonID = (int)reader["ApplicantPersonID"],
                     ApplicationDate = (DateTime)reader["ApplicationDate"],
                     ApplicationTypeID = (int)reader["ApplicationTypeID"],
                     ApplicationStatus = (byte)reader["ApplicationStatus"],
                     LastStatusDate = (DateTime)reader["LastStatusDate"],
                     PaidFees = (decimal)reader["PaidFees"],
                     CreatedByUserID = (int)reader["CreatedByUserID"]
                 };
             });
            return Application;
        }
        public static DataTable GetAllApplications()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM v_ApplicationsList");
        }
        public static int AddNewApplication(ApplicationDTO dto)
        {
            string query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                            VALUES 
                            (@PersonID, @Date, @TypeID, @Status, @StatusDate, @Fees, @UserID);
                            SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query,
                   cmd =>
                   {
                       cmd.Parameters.AddWithValue("@PersonID", dto.ApplicantPersonID);
                       cmd.Parameters.AddWithValue("@Date", dto.ApplicationDate);
                       cmd.Parameters.AddWithValue("@TypeID", dto.ApplicationTypeID);
                       cmd.Parameters.AddWithValue("@Status", dto.ApplicationStatus);
                       cmd.Parameters.AddWithValue("@StatusDate", dto.ApplicationDate);
                       cmd.Parameters.AddWithValue("@Fees", dto.PaidFees);
                       cmd.Parameters.AddWithValue("@UserID", dto.CreatedByUserID);
                   }
                   );
        }
        public static bool UpdateApplication(ApplicationDTO dto)
        {
            string query = @"UPDATE Applications SET 
                            ApplicantPersonID=@PersonID, 
                            ApplicationDate=@Date, 
                            ApplicationTypeID=@TypeID, 
                            ApplicationStatus=@Status, 
                            LastStatusDate=@StatusDate, 
                            PaidFees=@Fees, 
                            CreatedByUserID=@UserID
                            WHERE ApplicationID=@ID";
            return clsSqlHelper.ExecuteNonQuery(query,
               cmd =>
               {
                   cmd.Parameters.AddWithValue("@ID", dto.ApplicationID);
                   cmd.Parameters.AddWithValue("@PersonID", dto.ApplicantPersonID);
                   cmd.Parameters.AddWithValue("@Date", dto.ApplicationDate);
                   cmd.Parameters.AddWithValue("@TypeID", dto.ApplicationTypeID);
                   cmd.Parameters.AddWithValue("@Status", dto.ApplicationStatus);
                   cmd.Parameters.AddWithValue("@StatusDate", dto.LastStatusDate);
                   cmd.Parameters.AddWithValue("@Fees", dto.PaidFees);
                   cmd.Parameters.AddWithValue("@UserID", dto.CreatedByUserID);
               }
              ) > 0;
        }
        public static bool UpdateApplicationStatus(int applicationID, byte newStatus)
        {
            string query = @"UPDATE Applications SET ApplicationStatus=@NewStatus, LastStatusDate=@NewStatusDate WHERE ApplicationID=@ID";

            return clsSqlHelper.ExecuteNonQuery(query,
               cmd =>
               {
                   cmd.Parameters.AddWithValue("@ID", applicationID);
                   cmd.Parameters.AddWithValue("@NewStatus", newStatus);
                   cmd.Parameters.AddWithValue("@NewStatusDate", DateTime.Now);
               }

              ) > 0;
        }
        public static bool DeleteApplication(int AppID)
        {
            string query = @"DELETE FROM Applications WHERE ApplicationID = @ID";

            int rowsAffected = clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", AppID);
                });

            return rowsAffected > 0;
        }
        public static bool IsApplicationExist(int applicationID)
        {
            return clsSqlHelper.IsExist("SELECT Found=1 FROM Applications WHERE ApplicationID = @ID",
                 cmd => cmd.Parameters.AddWithValue("@ID", applicationID));
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            string query = @"SELECT Found=1 FROM Applications 
                             WHERE ApplicantPersonID = @PersonID 
                             AND ApplicationTypeID = @AppTypeID 
                             AND ApplicationStatus = 1"; // 1 = New
            return clsSqlHelper.IsExist(query,
                 cmd =>
                 {
                     cmd.Parameters.AddWithValue("@PersonID", PersonID);
                     cmd.Parameters.AddWithValue("@AppTypeID", ApplicationTypeID);
                 });
        }
        public static int GetActiveApplicationIDForLicenseClass(int personID, int licenseClassID, int applicationTypeID)
        {
            int applicationID = -1;

            string query = @"SELECT TOP 1 App.ApplicationID FROM Applications App
                           INNER JOIN LocalDrivingLicenseApplications LDLA 
                           ON App.ApplicationID = LDLA.ApplicationID
                           WHERE App.ApplicantPersonID = @PersonID
                           AND LDLA.LicenseClassID = @LicenseClassID
                           AND App.ApplicationTypeID = @ApplicationTypeID
                           AND App.ApplicationStatus = 1 
                           ORDER BY App.ApplicationDate DESC"; // New only

            clsSqlHelper.ExecuteReader(query,
            cmd =>
            {
            cmd.Parameters.AddWithValue("@PersonID", personID);
            cmd.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
            },
            reader =>
            {
            applicationID = (int)reader["ApplicationID"];
            });

            return applicationID;
        }

    }
}
