using System;

namespace SQLConsole.BizLogic.TaskRunning
{
    public class TaskEventArgs : EventArgs
    {
        public TaskResult Result { get; set; }
    }
}
