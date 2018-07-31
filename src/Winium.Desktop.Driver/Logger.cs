using log4net;
using log4net.Config;
using System.IO;

namespace Winium.Desktop.Driver
{
    internal static class Logger
    {
        private static readonly ILog LogInstance = LogManager.GetLogger(".WiniumLogger");

        public static ILog Log => LogInstance;

        internal static void LoadConfig(string logConfig)
        {
            var logFile = new FileInfo(logConfig);
            XmlConfigurator.Configure(logFile);
        }

        internal static void Silence()
        {
            LogManager.ResetConfiguration();
        }
    }
}