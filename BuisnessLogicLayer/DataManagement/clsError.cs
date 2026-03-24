using System.Data;
using DataAccessLayer.DataSources;

namespace BuisnessLogicLayer.DataManagement
{
    public class clsError
    {
        public static DataTable GetErrorLogs()
        {
            return clsErrorData.GetAllLogs();
        }
    }
}
