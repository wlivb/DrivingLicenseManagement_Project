using System.Configuration; 

namespace DataAccessLayer
{
    public static class clsDataAccessSettings 
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
}