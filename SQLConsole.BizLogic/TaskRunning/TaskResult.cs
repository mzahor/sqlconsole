using System;

namespace SQLConsole.BizLogic.TaskRunning
{
    public class TaskResult
    {
        public TimeSpan TimeTaken { get; set; }
        public Exception ExceptionThrown { get; set; }
        public bool WasExceptionThrown { get; set; }
        public int IterationsFinished { get; set; }
        public bool ExecutionFailed { get; set; }

        // for future use
        public object ActionResult { get; set; }
    }
}