using DVLD_DTOs;
using System.Data;

namespace DataAccessLayer.DataSources
{
    public class clsTestTypeData
    {
        public static TestTypeDTO GetTestTypeInfoByID(int ID)
        {
            TestTypeDTO TestType = null;

            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @ID";

            clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                },
                reader =>
                {
                    TestType = new TestTypeDTO
                    {
                        TestTypeID = (int)reader["TestTypeID"],
                        TestTypeTitle = (string)reader["TestTypeTitle"],
                        TestTypeDescription = (string)reader["TestTypeDescription"],
                        TestTypeFees = (decimal)reader["TestTypeFees"]
                    };
                });

            return TestType;
        }
        public static TestTypeDTO GetTestTypeInfoByTitle(string Title)
        {
            TestTypeDTO TestType = null;

            string query = "SELECT * FROM TestTypes WHERE TestTypetitle = @Title";

            clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Title", Title);
                },
                reader =>
                {
                    TestType = new TestTypeDTO
                    {
                        TestTypeID = (int)reader["TestTypeID"],
                        TestTypeTitle = (string)reader["TestTypeTitle"],
                        TestTypeDescription = (string)reader["TestTypeDescription"],
                        TestTypeFees = (decimal)reader["TestTypeFees"]
                    };
                });

            return TestType;
        }
        public static DataTable GetAllTestType()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM TestTypes");
        }
        public static int AddNewTestType(TestTypeDTO dto)
        {
            string query = @"INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
                         VALUES (@Title, @Description, @Fees);
                         SELECT SCOPE_IDENTITY();";
         return clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@TestTypeTitle", dto.TestTypeTitle);
                    cmd.Parameters.AddWithValue("@TestTypeDescription", dto.TestTypeDescription);
                    cmd.Parameters.AddWithValue("@TestTypeFees", dto.TestTypeFees);
                });
            
        }
        public static bool UpdateTestType(TestTypeDTO dto)
        {
            string query = @"UPDATE TestTypes SET TestTypeTitle=@Title, TestTypeDescription =@Description, TestTypeFees=@Fees WHERE TestTypeID=@ID";

            return clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", dto.TestTypeID);
                    cmd.Parameters.AddWithValue("@Title", dto.TestTypeTitle);
                    cmd.Parameters.AddWithValue("@Description", dto.TestTypeDescription);
                    cmd.Parameters.AddWithValue("@Fees", dto.TestTypeFees);
                }

               ) > 0;
        }
    }
}
