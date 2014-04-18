using System;

namespace SqlConsole.BizLogic.TaskRunning
{
    public class ExecutingResults
    {
        public TimeSpan ExecutingTime { get; set; }
        public ExecutingResults(double seconds)
        {
            ExecutingTime = TimeSpan.FromSeconds(seconds);
        }
    }
}
