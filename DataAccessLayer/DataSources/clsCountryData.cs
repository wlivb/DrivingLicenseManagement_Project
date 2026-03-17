using DVLD_DTOs;
using System.Data;

namespace DataAccessLayer.DataSources
{
    public static class clsCountryData
    {
        public static CountryDTO GetCountryInfoByID(int CountryID)
        {
            CountryDTO country = null;

            string query = "SELECT * FROM Countries WHERE CountryID = @ID";

            clsSqlHelper.ExecuteReader(query,
               cmd =>
               {
                   cmd.Parameters.AddWithValue("@ID", CountryID);
               },
               reader =>
               {
                   country = new CountryDTO
                   (
                   (int)reader["CountryID"], (string)reader["CountryName"]
                   );
               }
           );

            return country;
        }
        public static CountryDTO GetCountryInfoByName(string CountryName)
        {
            CountryDTO country = null;

            string query = "SELECT * FROM Countries WHERE CountryName = @Name";

            clsSqlHelper.ExecuteReader(query,
               cmd =>
               {
                   cmd.Parameters.AddWithValue("@Name", CountryName);
               },
               reader =>
               {
                   country = new CountryDTO
                   (
                   (int)reader["CountryID"], (string)reader["CountryName"]
                   );
               }
           );

            return country;
        }

        public static string GetCountryNameByID(int ID)
        {
            string CountryName = "";

            object result = clsSqlHelper.ExecuteScalar(
                "SELECT CountryName FROM Countries WHERE CountryID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", ID)
            );

            if (result != null)
            {
                CountryName = result.ToString();
            }

            return CountryName;
        }

        public static DataTable GetAllCountries()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM Countries");
        }
        public static int AddNewCountry(CountryDTO dto)
        {
            string query = @"INSERT INTO Countries (CountryName)
                         VALUES (@Name);
                         SELECT SCOPE_IDENTITY();";

            return clsSqlHelper.ExecuteScalar(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", dto.CountryName);
            });
        }
        public static bool UpdateCountry(CountryDTO dto)
        {
            string query = @"UPDATE Countries SET CountryName=@Name WHERE CountryID=@ID";

            return clsSqlHelper.ExecuteNonQuery(query,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@ID", dto.CountryID);
                }

               ) > 0;
        }
        public static bool DeleteCountry(int CountryID)
        {
            return clsSqlHelper.ExecuteNonQuery("DELETE FROM Countries WHERE CountryID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", CountryID)) > 0;
        }

    }

}
