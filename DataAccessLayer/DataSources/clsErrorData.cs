using System.Data;

namespace DataAccessLayer.DataSources
{
    public class clsErrorData
    {
        public static DataTable GetAllLogs()
        {
            return clsSqlHelper.ExecuteDataTable("SELECT * FROM ErrorLogs ORDER BY Timestamp DESC");
        }
    }
}
