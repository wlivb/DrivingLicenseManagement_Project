using DVLD_DTOs;
using System.Data;

namespace DataAccessLayer.DataSources
{
    public class clsApplictionTypeData
    {
        public static ApplicationTypeDTO GetApplicationTypeInfoByID(int ID)
        {
            ApplicationTypeDTO ApplicationType = null;

            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ID";

            clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                },
                reader =>
                {
                    ApplicationType = new ApplicationTypeDTO
                    {
                        ApplicationTypeID = (int)reader["ApplicationTypeID"],
                        ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"],
                        ApplicationTypeFees = (decimal)reader["ApplicationTypeFees"]
                    };
                });

           return ApplicationType;
        }
        public static ApplicationTypeDTO GetApplicationTypeInfoByTitle(string Title)
        {
            ApplicationTypeDTO ApplicationType = null;

            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypetitle = @Title";

            clsSqlHelper.ExecuteReader(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Title", Title);
                },
                reader =>
                {
                    ApplicationType = new ApplicationTypeDTO
                    {
                        ApplicationTypeID = (int)reader["ApplicationTypeID"],
                        ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"],
                        ApplicationTypeFees = (decimal)reader["ApplicationTypeFees"]
                    };
                });

            return ApplicationType;
        }
        public static DataTable GetAllApplicationType()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM ApplicationTypes");
        }
        public static int AddNewApplicationType(ApplicationTypeDTO dto)
        {
            string query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationTypeFees)
                         VALUES (@Title, @Fees);
                         SELECT SCOPE_IDENTITY();";
            return clsSqlHelper.ExecuteScalar(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Title", dto.ApplicationTypeTitle);
                    cmd.Parameters.AddWithValue("@Fees", dto.ApplicationTypeFees);
                });
        }
        public static bool UpdateApplicationType(ApplicationTypeDTO dto)
        {
            string query = @"UPDATE ApplicationTypes SET ApplicationTypeTitle=@Title, ApplicationTypeFees=@Fees WHERE ApplicationTypeID=@ID";

            return clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", dto.ApplicationTypeID);
                    cmd.Parameters.AddWithValue("@Title", dto.ApplicationTypeTitle);
                    cmd.Parameters.AddWithValue("@Fees", dto.ApplicationTypeFees);
                }

               ) > 0;
        }
    }
}
