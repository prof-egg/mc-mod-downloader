namespace MikManager.Utility
{
    public static class Debug
    {
        public static void LogError(string message, string logger = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (logger.Length > 0) logger += "/";
            Console.WriteLine($"[{GetTimestamp()}] [{logger}ERROR]: {message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void LogWarning(string message, string logger = "")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (logger.Length > 0) logger += "/";
            Console.WriteLine($"[{GetTimestamp()}] [{logger}WARNING]: {message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void LogInfo(string message, string logger = "")
        {   
            if (logger.Length > 0) logger += "/";
            Console.WriteLine($"[{GetTimestamp()}] [{logger}INFO]: {message}");
        }

        private static string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss");
        }
    }
}