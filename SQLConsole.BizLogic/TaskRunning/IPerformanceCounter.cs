using System;

namespace SQLConsole.BizLogic.TaskRunning
{
    public interface IPerformanceCounter
    {
        TimeSpan GetExecutingTime(Action action);
    }
}