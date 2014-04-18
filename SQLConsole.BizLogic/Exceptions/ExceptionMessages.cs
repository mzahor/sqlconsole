namespace SqlConsole.BizLogic.Exceptions
{
    public static class ExceptionMessages
    {
        public const string CouldNotOpenSqlConnection = @"Could not open sql connection.";
        public const string TaskRunnerIsAlreadyRunning = @"Task runner is already running some tasks.";
        public const string SqlErrorOccured = @"An error occured while running script: {0}";
        public const string UnknownErrorOccured = @"Unknown problem occured while executing the script: {0}";

        public const string ConfigurationSectionNotFound = "Configuration section not found...";
        public const string ConfigurationError = "Configuration error. See inner ex data for details.";
        public const string SqlScriptParsingError = "SqlScript parsing failed.";

        public const string TimerTaskError = "Timer task error";
    }
}
