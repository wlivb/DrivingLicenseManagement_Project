using System;
using System.Diagnostics;

namespace DataAccessLayer
{
    public static class clsLogger
    {
        private static string _sourceName = "DVLD_Project";
        public static void LogError(string message)
        {
            Log(message, EventLogEntryType.Error);
        }
        public static void LogWarning(string message)
        {
            Log(message, EventLogEntryType.Warning);
        }
        private static void Log(string message, EventLogEntryType type)
        {
            try
            {
                if (!EventLog.SourceExists(_sourceName))
                {
                    EventLog.CreateEventSource(_sourceName, "Application");
                }

                EventLog.WriteEntry(_sourceName, message, type);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Logger Failed: " + ex.Message);
            }
        }
    }
}
