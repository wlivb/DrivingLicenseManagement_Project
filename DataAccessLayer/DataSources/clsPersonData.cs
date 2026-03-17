using DVLD_DTOs;
using System;
using System.Data;

namespace DataAccessLayer.DataSources
{
    public static class clsPersonData
    {
        public static PersonDTO GetPersonInfoByID(int PersonID)
        {
            PersonDTO person = null;

            string query = "SELECT * FROM People WHERE PersonID = @ID";

             clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", PersonID);
                },
                reader =>
                {
                    person = new PersonDTO
                    {
                        PersonID = (int)reader["PersonID"],
                        NationalNo = (string)reader["NationalNo"],
                        FirstName = (string)reader["FirstName"],
                        SecondName = (string)reader["SecondName"],
                        ThirdName = (reader["ThirdName"] == DBNull.Value) ? "" : (string)reader["ThirdName"],
                        LastName = (string)reader["LastName"],
                        DateOfBirth = (System.DateTime)reader["DateOfBirth"],
                        Gendor = (byte)reader["Gender"],
                        Address = (string)reader["Address"],
                        Phone = (string)reader["Phone"],
                        Email = (string)reader["Email"],
                        NationalityCountryID = (int)reader["NationalityCountryID"],
                        ImagePath = (reader["ImagePath"] == DBNull.Value) ? "" : (string)reader["ImagePath"],
                    };
                }
            );

            return person;
        }
        public static PersonDTO GetPersonInfoByNationalNo(string NationalNo)
        {
            PersonDTO person = null;

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            clsSqlHelper.ExecuteReader(query,
               cmd =>
               {
                   cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
               },
               reader =>
               {
                   person = new PersonDTO
                   {
                       PersonID = (int)reader["PersonID"],
                       NationalNo = (string)reader["NationalNo"],
                       FirstName = (string)reader["FirstName"],
                       SecondName = (string)reader["SecondName"],
                       ThirdName = (reader["ThirdName"] == DBNull.Value) ? "" : (string)reader["ThirdName"],
                       LastName = (string)reader["LastName"],
                       DateOfBirth = (System.DateTime)reader["DateOfBirth"],
                       Gendor = (byte)reader["Gender"],
                       Address = (string)reader["Address"],
                       Phone = (string)reader["Phone"],
                       Email = (string)reader["Email"],
                       NationalityCountryID = (int)reader["NationalityCountryID"],
                       ImagePath = (reader["ImagePath"] == DBNull.Value) ? "" : (string)reader["ImagePath"],
                   };
               }
           );

            return person;
        }
        public static int AddNewPerson(PersonDTO dto)
        {
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, 
                         DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath)
                         VALUES (@Nat, @F, @S, @T, @L, @DOB, @G, @Addr, @Ph, @Em, @CID, @Img);
                         SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query, cmd => {
                cmd.Parameters.AddWithValue("@Nat", dto.NationalNo);
                cmd.Parameters.AddWithValue("@F", dto.FirstName);
                cmd.Parameters.AddWithValue("@S", dto.SecondName);
                cmd.Parameters.AddWithValue("@T", (object)dto.ThirdName ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@L", dto.LastName);
                cmd.Parameters.AddWithValue("@DOB", dto.DateOfBirth);
                cmd.Parameters.AddWithValue("@G", dto.Gendor);
                cmd.Parameters.AddWithValue("@Addr", dto.Address);
                cmd.Parameters.AddWithValue("@Ph", dto.Phone);
                cmd.Parameters.AddWithValue("@Em", (object)dto.Email ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@CID", dto.NationalityCountryID);
                cmd.Parameters.AddWithValue("@Img", (object)dto.ImagePath ?? System.DBNull.Value);
            });
        }
        public static bool UpdatePerson(PersonDTO dto)
        {
            string query = @"UPDATE People SET NationalNo=@Nat, FirstName=@F, SecondName=@S, ThirdName=@T, 
                         LastName=@L, DateOfBirth=@DOB, Gender=@G, Address=@Addr, Phone=@Ph, 
                         Email=@Em, NationalityCountryID=@CID, ImagePath=@Img WHERE PersonID=@ID";

            return clsSqlHelper.ExecuteNonQuery(query, 
                cmd => 
                {
                cmd.Parameters.AddWithValue("@ID", dto.PersonID);
                cmd.Parameters.AddWithValue("@Nat", dto.NationalNo);
                cmd.Parameters.AddWithValue("@F", dto.FirstName);
                cmd.Parameters.AddWithValue("@S", dto.SecondName);
                cmd.Parameters.AddWithValue("@T", (object)dto.ThirdName ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@L", dto.LastName);
                cmd.Parameters.AddWithValue("@DOB", dto.DateOfBirth);
                cmd.Parameters.AddWithValue("@G", dto.Gendor);
                cmd.Parameters.AddWithValue("@Addr", dto.Address);
                cmd.Parameters.AddWithValue("@Ph", dto.Phone);
                cmd.Parameters.AddWithValue("@Em", (object)dto.Email ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@CID", dto.NationalityCountryID);
                cmd.Parameters.AddWithValue("@Img", (object)dto.ImagePath ?? System.DBNull.Value);
                }

               ) > 0;
        }
        public static bool DeletePerson(int PersonID)
        {
            return clsSqlHelper.ExecuteNonQuery("DELETE FROM People WHERE PersonID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", PersonID)) > 0;
        }
        public static bool IsPersonLinkedToOtherRecords(int PersonID)
        {
            string query = @"SELECT Found=1 WHERE 
                           EXISTS (SELECT 1 FROM Users WHERE PersonID = @ID) 
                           OR 
                           EXISTS (SELECT 1 FROM Drivers WHERE PersonID = @ID)
                           OR
                           EXISTS (SELECT 1 FROM Applications WHERE ApplicantPersonID = @ID)";

            return clsSqlHelper.IsExist(query, cmd => {
                cmd.Parameters.AddWithValue("@ID", PersonID);
            });
        }
        public static DataTable GetAllPeople()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM v_PeopleList");
        }
        public static bool IsPersonExist(int PersonID)
        {
            return clsSqlHelper.IsExist("SELECT Found=1 FROM People WHERE PersonID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", PersonID));
        }
        public static bool IsPersonExist(string NationalNo)
        {
            return clsSqlHelper.IsExist("SELECT Found=1 FROM People WHERE NationalNo = @Nat",
                cmd => cmd.Parameters.AddWithValue("@Nat", NationalNo));
        }
    }
}
