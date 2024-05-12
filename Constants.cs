namespace oksei_fsot_api
{
    public static class Constants
    {
        public static readonly string serverUrl = "https://16fb-185-7-93-247.ngrok-free.app";

        public static readonly string localPathToStorages = @"Resources/";
        public static readonly string localPathToProfileIcons = $"{localPathToStorages}ProfileIcons/";

        public static readonly string webPathToProfileIcons = $"{serverUrl}/api/upload/profileIcon/";
        public static readonly string webPathToReports = $"{serverUrl}/api/report/file/";


        private static readonly string pathToLogDirectory = "Logs/";
        public static readonly string pathToMarkLogs = $"{pathToLogDirectory}MarkLogs/";
        public static readonly string pathToMarkAnnualLogs = $"{pathToLogDirectory}MarkLogs/Annual/";

        private static readonly string pathToReportDirectory = "Reports/";
        public static readonly string pathToTeacherReports = $"{pathToReportDirectory}TeacherReports/";
        public static readonly string pathToTeacherAnnualReports = $"{pathToReportDirectory}TeacherReports/Annual/";
    }
}