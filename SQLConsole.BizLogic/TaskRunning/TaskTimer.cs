using System.Timers;

namespace SQLConsole.BizLogic.TaskRunning
{
    public class TaskTimer : Timer
    {
        public object TaskParameter { get; set; }
    }
}