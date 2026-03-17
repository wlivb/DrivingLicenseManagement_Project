using DVLD_DTOs;
using System;
using System.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace DataAccessLayer.DataSources
{
    public class clsTestAppointmentData
    {
        public static TestAppointmentDTO GetTestAppointmentInfoByID(int testAppointmentId)
        {
            TestAppointmentDTO dto = null;

            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @ID";

            clsSqlHelper.ExecuteReader(query,
                        cmd =>
                        {
                            cmd.Parameters.AddWithValue("@ID", testAppointmentId);
                        },
                        reader =>
                        {
                                dto = new TestAppointmentDTO
                                {
                                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]),
                                    TestTypeID = Convert.ToInt32(reader["TestTypeID"]),
                                    LocalDrivingLicenseAppID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]),
                                    AppointmentDate = (DateTime)reader["AppointmentDate"],
                                    PaidFees = Convert.ToDecimal(reader["PaidFees"]),
                                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]),
                                    IsLocked = (bool)reader["IsLocked"],
                                    RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["RetakeTestApplicationID"])
                                };
                            
                        });

            return dto;
        }
        public static TestAppointmentDTO GetTestAppointmentInfoByTestTypeID(int testTypeId)
        {
            TestAppointmentDTO dto = null;

            string query = "SELECT * FROM TestAppointments WHERE TestTypeID = @ID";

            clsSqlHelper.ExecuteReader(query,
                        cmd =>
                        {
                            cmd.Parameters.AddWithValue("@ID", testTypeId);
                        },
                        reader =>
                        {
                                dto = new TestAppointmentDTO
                                {
                                    TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]),
                                    TestTypeID = Convert.ToInt32(reader["TestTypeID"]),
                                    LocalDrivingLicenseAppID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]),
                                    AppointmentDate = (DateTime)reader["AppointmentDate"],
                                    PaidFees = Convert.ToDecimal(reader["PaidFees"]),
                                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]),
                                    IsLocked = (bool)reader["IsLocked"],
                                    RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["RetakeTestApplicationID"])
                                };
                            
                        });

            return dto;
        }
        public static TestAppointmentDTO GetLastTestAppointment(int localAppId, int testTypeId, ref bool isFound)
        {
            bool isfound = false;

            TestAppointmentDTO dto = null;

            string query = @"SELECT top 1 * FROM TestAppointments
                           WHERE (TestTypeID = @TestTypeID) 
                           AND   (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                           order by TestAppointmentID Desc";

            clsSqlHelper.ExecuteReader(query,
                         cmd =>
                         {
                             cmd.Parameters.AddWithValue("@TestTypeID", testTypeId);
                             cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localAppId);
                         },
                         reader =>
                         {
                             dto = new TestAppointmentDTO
                             {
                                 TestAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]),
                                 TestTypeID = Convert.ToInt32(reader["TestTypeID"]),
                                 LocalDrivingLicenseAppID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]),
                                 AppointmentDate = (DateTime)reader["AppointmentDate"],
                                 PaidFees = Convert.ToDecimal(reader["PaidFees"]),
                                 CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]),
                                 IsLocked = (bool)reader["IsLocked"],
                                 RetakeTestApplicationID = (reader["RetakeTestApplicationID"] == DBNull.Value) ? -1 : Convert.ToInt32(reader["RetakeTestApplicationID"])
                             };

                             isfound = true;
                             
                         });

            isFound = isfound;

            return dto;

        }
        public static DataTable GetAllTestAppointments()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM v_TestAppointmentsList order by AppointmentDate Desc");
        }
        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            string query = @"SELECT TestAppointmentID, AppointmentDate,PaidFees, IsLocked
                           FROM TestAppointments
                           WHERE LocalDrivingLicenseApplicationID = @ID
                           AND TestTypeID = @TestTypeID
                           order by AppointmentDate Desc;";

            return clsSqlHelper.ExecuteDataTable(query,
                        cmd =>
                        {
                            cmd.Parameters.AddWithValue("@ID", LocalDrivingLicenseApplicationID);
                            cmd.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        });
        }
        public static int AddNewTestAppointment(TestAppointmentDTO dto)
        {
            int ID = -1;

            string query = @"INSERT INTO TestAppointments 
                    (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
                    VALUES 
                    (@TestTypeID, @LocalAppID, @Date, @Fees, @UserID, @IsLocked, @RetakeAppID);
                    SELECT SCOPE_IDENTITY();";

            ID = clsSqlHelper.ExecuteScalar(query,
                   cmd =>
                   {
                       cmd.Parameters.AddWithValue("@TestTypeID", dto.TestTypeID);
                       cmd.Parameters.AddWithValue("@LocalAppID", dto.LocalDrivingLicenseAppID);
                       cmd.Parameters.AddWithValue("@Date", dto.AppointmentDate);
                       cmd.Parameters.AddWithValue("@Fees", dto.PaidFees);
                       cmd.Parameters.AddWithValue("@UserID", dto.CreatedByUserID);
                       cmd.Parameters.AddWithValue("@IsLocked", dto.IsLocked);

                       if (dto.RetakeTestApplicationID == -1)
                       {
                           cmd.Parameters.AddWithValue("@RetakeAppID", DBNull.Value);
                       }
                       else
                       {
                           cmd.Parameters.AddWithValue("@RetakeAppID", dto.RetakeTestApplicationID);
                       }
                   });

            return ID;
        }
        public static bool UpdateTestAppointment(TestAppointmentDTO dto)
        {
            string query = @"UPDATE TestAppointments SET 
                            TestTypeID=@TestTypeID, 
                            LocalDrivingLicenseApplicationID=@LocalAppID, 
                            AppointmentDate=@Date, 
                            PaidFees=@Fees, 
                            CreatedByUserID=@UserID, 
                            IsLocked=@IsLocked, 
                            RetakeTestApplicationID=@RetakeAppID
                            WHERE TestAppointmentID=@ID";

            int rowsAffected = clsSqlHelper.ExecuteNonQuery(query,
                   cmd =>
                   {
                       cmd.Parameters.AddWithValue("@TestTypeID", dto.TestTypeID);
                       cmd.Parameters.AddWithValue("@LocalAppID", dto.LocalDrivingLicenseAppID);
                       cmd.Parameters.AddWithValue("@Date", dto.AppointmentDate);
                       cmd.Parameters.AddWithValue("@Fees", dto.PaidFees);
                       cmd.Parameters.AddWithValue("@UserID", dto.CreatedByUserID);
                       cmd.Parameters.AddWithValue("@IsLocked", dto.IsLocked);
                       cmd.Parameters.AddWithValue("@RetakeAppID", (dto.RetakeTestApplicationID == -1) ? (object)DBNull.Value : dto.RetakeTestApplicationID);
                       cmd.Parameters.AddWithValue("@ID", dto.TestAppointmentID);
                   });
            return rowsAffected > 0;
        }
        public static int GetTestID(int testAppointment)
        {
            string query = @"SELECT TestID FROM Tests WHERE TestAppointmentID = @ID";

            return clsSqlHelper.ExecuteScalar(query,
                        cmd =>
                        {
                            cmd.Parameters.AddWithValue("@ID", testAppointment);
                        });
        }
        public static bool LockTestAppointment(int testAppointmentID)
        {
            string query = @"UPDATE TestAppointments SET IsLocked = 1 WHERE TestAppointmentID = @ID";

            int rowsAffected = clsSqlHelper.ExecuteNonQuery(query, 
                cmd => 
                {
                cmd.Parameters.AddWithValue("@ID", testAppointmentID);
                });
            return rowsAffected > 0;
        }
    }
}
