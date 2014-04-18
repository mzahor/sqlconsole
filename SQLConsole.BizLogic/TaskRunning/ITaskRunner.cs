using System;
using System.Collections.Concurrent;
using SQLConsole.BizLogic.TaskRunning;

namespace SqlConsole.BizLogic.TaskRunning
{
    public interface ITaskRunner
    {
        int IterationsCount { get; set; }
        int TasksCount { get; set; }
        
        int TasksFinished { get; }
        int TotalIterationsFinished { get; }
        
        int TotalIterationsCount { get; }
        int TasksRemaining { get; }
        int TotalIterationsRemaining { get; }
        
        double TotalProgress { get; }
        bool IsRunning { get; }
        TaskRunnerStatus Status { get; }
        Action ActionToRun { get; set; }
        ConcurrentBag<TaskResult> Results { get; }
        TimeSpan TotalTimeTaken { get; }
        TimeSpan ExtrapolatedTimeToFinish { get; }
        void StartRunning(Action action);
        void StartRunning();
        void StopRunning();
        void Join();
        event EventHandler<TaskEventArgs> TaskFinished;
        event EventHandler ExecutionFinished;
    }
}