using DVLD_DTOs;
using System;
using System.Data;

namespace DataAccessLayer.DataSources
{
    public class clsTestData
    {
        public static TestDTO GetTestInfoByID(int testID)
        {
            TestDTO dto = null;

            string query = "Select * From Tests where TestID = @ID";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@ID", testID);
                         },
                         reader =>
                         {
                             dto = new TestDTO
                             {
                                 TestID = (int)reader["TestID"],
                                 TestAppointmentID = (int)reader["TestAppointmentID"],
                                 TestResult = Convert.ToByte(reader["TestResult"]),
                                 Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
                                 CreatedByUserID = (int)reader["CreatedByUserID"]
                             };
                         });

            return dto;
        }
        public static TestDTO GetLastTest(int personID, int licenseClassID, int testTypeID)
        {
            TestDTO dto = null;

            string query = @"SELECT TOP 1 
                             Tests.TestID, 
                             Tests.TestAppointmentID, 
                             Tests.TestResult, 
                             Tests.Notes, 
                             Tests.CreatedByUserID, 
                             Applications.ApplicantPersonID
                             FROM Tests 
                             INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID 
                             INNER JOIN LocalDrivingLicenseApplications ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
                             INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             WHERE 
                             Applications.ApplicantPersonID = @PersonID 
                             AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID 
                             AND TestAppointments.TestTypeID = @TestTypeID
                             ORDER BY Tests.TestAppointmentID DESC";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@PersonID", personID);
                             cmd.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                             cmd.Parameters.AddWithValue("@TestTypeID", testTypeID);
                         },
                         reader =>
                         {
                             dto = new TestDTO
                             {
                                 TestID = (int)reader["TestID"],
                                 TestAppointmentID = (int)reader["TestAppointmentID"],
                                 TestResult = Convert.ToByte(reader["TestResult"]),
                                 Notes = (reader["Notes"] == DBNull.Value) ? "" : (string)reader["Notes"],
                                 CreatedByUserID = (int)reader["CreatedByUserID"]
                             };
                         });

            return dto;
        }
        public static DataTable GetAllTests()
        {
            return clsSqlHelper.ExecuteDataTable("Select * from Tests order by TestID");
        }
        public static int AddNewTest(TestDTO dto)
        {
            string query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                             VALUES 
                             (@AppointmentID, @Result, @Notes, @UserID);
                              
                              Update TestAppointments Set IsLocked = 1 Where TestAppointmentID = @AppointmentID;

                             SELECT SCOPE_IDENTITY();";
            return clsSqlHelper.ExecuteScalar(query,
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@AppointmentID", dto.TestAppointmentID);
                        cmd.Parameters.AddWithValue("@Result", dto.TestResult);
                        if (dto.Notes == "")
                        {
                            cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Notes", dto.Notes);
                        }
                        cmd.Parameters.AddWithValue("@UserID", dto.CreatedByUserID);
                    });
        }
        public static bool UpdateTest(TestDTO dto)
        {
            string query = @"UPDATE Tests SET TestResult=@Result, Notes=@Notes WHERE TestID=@ID";
            return clsSqlHelper.ExecuteNonQuery(query,
               cmd =>
               {
                   cmd.Parameters.AddWithValue("@ID", dto.TestID);
                   cmd.Parameters.AddWithValue("@Result", dto.TestResult);
                   if (dto.Notes == "")
                   {
                       cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                   }
                   else
                   {
                       cmd.Parameters.AddWithValue("@Notes", dto.Notes);
                   }

               }) > 0;

        }
        public static byte GetPassedTestCount(int localAppID)
        {
            string query = @"SELECT PassedTestCount = count(TestTypeID)
                         FROM Tests INNER JOIN
                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						 where LocalDrivingLicenseApplicationID =@LocalAppID and TestResult=1";

            return (byte) clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@LocalAppID", localAppID);
                });
        }
    }
}
