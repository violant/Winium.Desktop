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
            var result = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args);
            switch (result.Tag)
            {
                case ParserResultType.Parsed:
                    var parsed = (Parsed<CommandLineOptions>)result;
                    var listeningPort = 9999;
                    var version = typeof(Program).Assembly.GetName().Version.ToString(3);

                    Logger.Log.Info($"Running Winium Server version {version}");
                    Logger.Log.Info($"Running from {Environment.CurrentDirectory}");
                    var options = parsed.Value;
                    if (options.Port.HasValue)
                    {
                        listeningPort = options.Port.Value;
                    }

                    if (!string.IsNullOrEmpty(options.LogConfig))
                        Logger.LoadConfig(options.LogConfig);
                    else if (options.Silent)
                        Logger.Silence();

                    try
                    {
                        var listener = new Listener(listeningPort);
                        Listener.UrnPrefix = options.UrlBase;
                        Logger.Log.Info($"Starting Windows Desktop Driver on port {listeningPort}");
                        listener.StartListening();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Fatal("Failed to start driver: {0}", ex);
                        throw;
                    }

                    break;
                case ParserResultType.NotParsed:
                    var notParsed = (NotParsed<CommandLineOptions>)result;
                    var errors = notParsed.Errors;
                    // do your stuff with errors here
                    Logger.Log.Error($"Failed to parse commanline parameters with errors: {errors}");
                    break;
            }
        }

        #endregion
    }
}
