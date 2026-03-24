namespace BuisnessLogicLayer
{
    public static class clsBllErrorBridge
    {
        public static void InitializeLogging()
        {
            DataAccessLayer.clsErrorLogger.Subscribe();
        }
    }
}
