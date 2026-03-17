using DVLD_DTOs;
using System.Data;

namespace DataAccessLayer.DataSources
{
    public class clsLicenseClassData
    {
        public static LicenseClassDTO GetLicenseClassByID(int licenseClassId)
        {

            LicenseClassDTO LicenseClass = null;

            string query = "Select * From LicenseClasses Where LicenseClassID = @ID";

            clsSqlHelper.ExecuteReader(query,

                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", licenseClassId);
                },
                reader =>
                {
                    LicenseClass = new LicenseClassDTO()
                    {
                        LicenseClassID = (int)reader["LicenseClassID"],
                        ClassName = (string)reader["ClassName"],
                        ClassDescription = (string)reader["ClassDescription"],
                        MinimumAllowedAge = (byte)reader["MinimumAllowedAge"],
                        DefaultValidityLength = (byte)reader["DefaultValidityLength"],
                        ClassFees = (decimal)reader["ClassFees"]
                    };
                });

            return LicenseClass;
        }
        public static LicenseClassDTO GetLicenseClassByClassName(string Name)
        {
            LicenseClassDTO LicenseClass = null;

            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @name";

            clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@name", Name);
                },
                reader =>
                {
                    LicenseClass = new LicenseClassDTO
                    {
                        LicenseClassID = (int)reader["LicenseClassID"],
                        ClassName = reader["ClassName"].ToString(),
                        ClassDescription = reader["ClassDescription"].ToString(),
                        MinimumAllowedAge = (byte)reader["MinimumAllowedAge"],
                        DefaultValidityLength = (byte)reader["DefaultValidityLength"],
                        ClassFees = (decimal)reader["ClassFees"]
                    };
                });

            return LicenseClass;
        }
        public static DataTable GetAllLicenseClass()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM LicenseClasses");
        }
        public static int AddNewLicenseClass(LicenseClassDTO dto)
        {
            string query = @"INSERT INTO LicenseClasses (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength,ClassFees)
                         VALUES (@Name, @Description, @MinAge, @ValidLength, @Fees);
                         SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", dto.ClassName);
                    cmd.Parameters.AddWithValue("@Description", dto.ClassDescription);
                    cmd.Parameters.AddWithValue("@MinAge", dto.MinimumAllowedAge);
                    cmd.Parameters.AddWithValue("@ValidLength", dto.DefaultValidityLength);
                    cmd.Parameters.AddWithValue("@Fees", dto.ClassFees);
                });
        }
        public static bool UpdateLicenseClass(LicenseClassDTO dto)
        {
            string query = @"UPDATE LicenseClasses SET ClassName=@Name, ClassDescription=@Description, MinimumAllowedAge = @MinAge, DefaultValidityLength = @ValidLength ,ClassFees=@Fees WHERE LicenseClassID=@ID";

            return clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", dto.LicenseClassID);
                    cmd.Parameters.AddWithValue("@Name", dto.ClassName);
                    cmd.Parameters.AddWithValue("@ClassDescription", dto.ClassDescription);
                    cmd.Parameters.AddWithValue("@MinAge", dto.MinimumAllowedAge);
                    cmd.Parameters.AddWithValue("@ValidLength", dto.DefaultValidityLength);
                    cmd.Parameters.AddWithValue("@Fees", dto.ClassFees);

                }) > 0;
        }
    }
}
