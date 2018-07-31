namespace Winium.Desktop.Driver
{
    #region using

    using CommandLine;
    using CommandLine.Text;

    #endregion

    internal class CommandLineOptions
    {
        #region Public Properties

        [Option("log-config", Required = false, HelpText = "path to the log4net config file, overriding the standard log config")]
        public string LogConfig { get; set; }

        [Option("port", Required = false, HelpText = "port to listen on")]
        public int? Port { get; set; }

        [Option("url-base", Required = false, HelpText = "base URL path prefix for commands, e.g. wd/url")]
        public string UrlBase { get; set; }

        [Option("silent", Required = false, HelpText = "log nothing")]
        public bool Silent { get; set; }

        #endregion

        #region Public Methods and Operators

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        #endregion
    }
}
