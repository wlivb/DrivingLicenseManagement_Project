using System;
using System.Data;
using DVLD_DTOs;

namespace DataAccessLayer.DataSources
{
    public class clsLocalDrivingLicenseAppData
    {
        public static LocalDrivingLicenseAppDTO GetLocalDrivingLicenseApplicationInfoByID(int LocalAppID)
        {
            LocalDrivingLicenseAppDTO LocalApp = null; 

            string query = @"SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @localAppId";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@localAppId", LocalAppID);
                         },
                         reader =>
                         {
                             LocalApp = new LocalDrivingLicenseAppDTO
                             {
                                 LocalDrivingLicenseAppID = Convert.ToInt32(reader["LocalDrivingLicenseAppID"]),
                                 ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                                 LicenseClassID = Convert.ToInt32(reader["LicenseClassID"])
                             };
                         });

            return LocalApp;
        }
        public static LocalDrivingLicenseAppDTO GetLocalDrivingLicenseAppInfoByApplicationID(int ApplicationID)
        {
            LocalDrivingLicenseAppDTO LocalApp = null;

            string query = @"SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @applicationId";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@applicationId", ApplicationID);
                         },
                         reader =>
                         {
                             LocalApp = new LocalDrivingLicenseAppDTO
                             {
                                 LocalDrivingLicenseAppID = Convert.ToInt32(reader["LocalDrivingLicenseAppID"]),
                                 ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                                 LicenseClassID = Convert.ToInt32(reader["LicenseClassID"])
                             };
                         });

            return LocalApp;
        }
        public static LocalDrivingLicenseAppFullInfoDTO GetLocalDrivingLicenseAppFullInfoByID(int ID)
        {
            LocalDrivingLicenseAppFullInfoDTO fullInfo = null;

            string query = @"SELECT * FROM LocalDrivingLicenseApplications 
                     INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                     WHERE LocalDrivingLicenseApplicationID = @ID";

            clsSqlHelper.ExecuteReader(query,
                cmd => cmd.Parameters.AddWithValue("@ID", ID),
                reader =>
                {
                    fullInfo = new LocalDrivingLicenseAppFullInfoDTO
                    {
                        Application = new ApplicationDTO(
                            (int)reader["ApplicationID"], (int)reader["ApplicantPersonID"],
                            (DateTime)reader["ApplicationDate"], (int)reader["ApplicationTypeID"],
                            (byte)reader["ApplicationStatus"], (DateTime)reader["LastStatusDate"],
                            (decimal)reader["PaidFees"], (int)reader["CreatedByUserID"]
                        ),
                        LocalApp = new LocalDrivingLicenseAppDTO(
                            (int)reader["LocalDrivingLicenseApplicationID"],
                            (int)reader["ApplicationID"], (int)reader["LicenseClassID"]
                        )
                    };
                });

            return fullInfo;
        }
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM v_LocalDrivingLicenseApplicationsList ");
        }
        public static int AddNewLocalDrivingLicenseApplication(LocalDrivingLicenseAppDTO dto)
        {
            string query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                             VALUES (@applicationId, @licenseClassId);
                             SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@applicationId", dto.ApplicationID);
                    cmd.Parameters.AddWithValue("@licenseClassId", dto.LicenseClassID);
                });
        }
        public static bool UpdateLocalDrivingLicenseApplication(LocalDrivingLicenseAppDTO dto)
        {
            string query = @"UPDATE LocalDrivingLicenseApplications
                             SET ApplicationID = @applicationId,
                                 LicenseClassID = @licenseClassId
                             WHERE LocalDrivingLicenseApplicationID = @localAppId";
            int rowsAffected = clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@applicationId", dto.ApplicationID);
                    cmd.Parameters.AddWithValue("@licenseClassId", dto.LicenseClassID);
                    cmd.Parameters.AddWithValue("@localAppId", dto.LocalDrivingLicenseAppID);
                });
            return rowsAffected > 0;
        }
        public static bool DeleteLocalDrivingLicenseApplication(int LocalAppID, int AppID)
        {
            // ملاحظة: أضفنا حذف الاختبارات والمواعيد أولاً لتجنب تعارض الـ FK
            string query = @"
            -- 1. حذف نتائج الاختبارات المرتبطة بمواعيد هذا الطلب
            DELETE FROM Tests 
            WHERE TestAppointmentID IN (SELECT TestAppointmentID FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @localAppId);
            
            -- 2. حذف مواعيد الاختبارات المرتبطة بالطلب المحلي
            DELETE FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @localAppId;
            
            -- 3. حذف الطلب من جدول الطلبات المحلية
            DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @localAppId;
            
            -- 4. حذف الطلب الأساسي من جدول الطلبات العام
            DELETE FROM Applications WHERE ApplicationID = @appId;";

            int rowsAffected = clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@localAppId", LocalAppID);
                    cmd.Parameters.AddWithValue("@appId", AppID);
                });

            return rowsAffected > 0;
        }
        public static bool DoesPassTestType(int localDrivingLicenseApplicationID, int testTypeId)
        {
            string query = @"SELECT TOP 1 1
                     FROM Tests 
                     INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                     WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LDLAppID
                     AND TestAppointments.TestTypeID = @TestTypeID
                     AND Tests.TestResult = 1"; // 1 تعني ناجح

            object result = clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@LDLAppID", localDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", testTypeId);
                });

            return (result != null); // إذا أعاد قيمة (1) فهو ناجح، إذا كان Null فهو لم ينجح بعد
        }
        public static bool DoesAttendTestType(int localDrivingLicenseApplicationID, int testTypeId)
        {
            string query = @"SELECT TOP 1 1 FROM TestAppointments 
                           WHERE LocalDrivingLicenseApplicationID = @LDLAppID
                           AND TestTypeID = @TestTypeID
                           AND IsLocked = 1";  // ← Locked = حضر

            return clsSqlHelper.IsExist(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@LDLAppID", localDrivingLicenseApplicationID);
                cmd.Parameters.AddWithValue("@TestTypeID", testTypeId);
            });
        }
        public static int TotalTrialsPerTest(int localDrivingLicenseApplicationID, int testTypeId)
        {
            string query = @"SELECT count(TestID)
                     FROM TestAppointments 
                     INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                     WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LDLApplicationID
                     AND TestAppointments.TestTypeID = @TestTypeID";

         return clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@LDLApplicationID", localDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", testTypeId);
                });
        }
        public static bool IsThereAnActiveScheduledTest(int localDrivingLicenseApplicationID, int testTypeId)
        {
            string query = @"SELECT TOP 1 1 FROM TestAppointments 
                           WHERE LocalDrivingLicenseApplicationID = @LDLAppID 
                           AND TestTypeID = @TestTypeID 
                           AND IsLocked = 0";

            return clsSqlHelper.IsExist(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@LDLAppID", localDrivingLicenseApplicationID);
                    cmd.Parameters.AddWithValue("@TestTypeID", testTypeId);
                });
        }
    }
}
