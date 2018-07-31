namespace Winium.Desktop.Driver
{
    #region using

    using System;

    using CommandLine;

    #endregion

    internal static class Program
    {
        #region Methods

        [STAThread]
        private static void Main(string[] args)
        {
            var listeningPort = 9999;
            var version = typeof(Program).Assembly.GetName().Version.ToString(3);
            var options = new CommandLineOptions();
            Logger.Log.Info($"Running remote web driver version {version}");
            Logger.Log.Info($"Running from {Environment.CurrentDirectory}");
            if (Parser.Default.ParseArguments(args, options))
            {
                if (options.Port.HasValue)
                {
                    listeningPort = options.Port.Value;
                }

                if (!string.IsNullOrEmpty(options.LogConfig))
                    Logger.LoadConfig(options.LogConfig);
                else if (options.Silent)
                    Logger.Silence();
            }

            try
            {
                var listener = new Listener(listeningPort);
                Listener.UrnPrefix = options.UrlBase;

                Console.WriteLine("Starting Windows Desktop Driver on port {0}\n", listeningPort);

                listener.StartListening();
            }
            catch (Exception ex)
            {
                Logger.Log.Fatal("Failed to start driver: {0}", ex);
                throw;
            }
        }

        #endregion
    }
}
