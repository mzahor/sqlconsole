using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SQLConsole.BizLogic.TaskRunning;
using SqlConsole.BizLogic.Exceptions;

namespace SqlConsole.BizLogic.TaskRunning
{
    public class TaskRunner : ITaskRunner
    {
        private readonly object _lock = new object();
        private readonly IPerformanceCounter _performanceCounter;
        private readonly List<Task<TaskResult>> _tasks;
        private Action _actionToRun;
        private int _iterationsCount;
        private ManualResetEvent _resetEvent;
        private ConcurrentBag<TaskResult> _results;
        private DateTime _startTime;
        private volatile TaskRunnerStatus _status;
        private volatile bool _stop;
        private DateTime _stopTime;
        private int _tasksCount;
        private volatile int _tasksFinished;
        private volatile int _totalIterationsFinished;

        public TaskRunner(IPerformanceCounter performanceCounter)
        {
            _status = TaskRunnerStatus.NotStarted;
            _performanceCounter = performanceCounter;
            _tasks = new List<Task<TaskResult>>();
            _results = new ConcurrentBag<TaskResult>();
            _stop = false;
        }

        #region ITaskRunner Members

        public int TasksRemaining
        {
            get { return _tasksCount - _tasksFinished; }
        }

        public int TotalIterationsRemaining
        {
            get { return TotalIterationsCount - _totalIterationsFinished; }
        }
        
        public double TotalProgress
        {
            get { return (double) _totalIterationsFinished / TotalIterationsCount; }
        }

        public int IterationsCount
        {
            get { return _iterationsCount; }
            set { _iterationsCount = value; }
        }

        public int TotalIterationsCount
        {
            get { return _tasksCount * _iterationsCount; }
        }

        public int TasksCount
        {
            get { return _tasksCount; }
            set { _tasksCount = value; }
        }

        public ConcurrentBag<TaskResult> Results
        {
            get { return _results; }
        }

        public TimeSpan TotalTimeTaken
        {
            get { return IsRunning ? DateTime.Now - _startTime : _stopTime - _startTime; }
        }

        public TimeSpan ExtrapolatedTimeToFinish
        {
            get { return TimeSpan.FromSeconds(TotalTimeTaken.TotalSeconds / TotalProgress); }
        }

        public int TasksFinished
        {
            get { return _tasksFinished; }
        }

        public int TotalIterationsFinished
        {
            get { return _totalIterationsFinished; }
        }

        public bool IsRunning
        {
            get { return _status == TaskRunnerStatus.Running; }
        }

        public TaskRunnerStatus Status
        {
            get { return _status; }
        }

        public Action ActionToRun
        {
            get { return _actionToRun; }
            set { _actionToRun = value; }
        }

        public void StartRunning(Action action)
        {
            InitializeCounters();

            _stop = false;
            lock (_lock)
            {
                if (IsRunning)
                {
                    throw new BizLogicException(ExceptionMessages.TaskRunnerIsAlreadyRunning);
                }

                _status = TaskRunnerStatus.Running;
            }

            _resetEvent = new ManualResetEvent(false);

            _actionToRun = action;

            _tasks.Clear();
            _results = new ConcurrentBag<TaskResult>();

            for (int i = 0; i < TasksCount; i++)
            {
                var task = new Task<TaskResult>(TaskRoutine, action);

                task.ContinueWith(results => OnTaskFinished(new TaskEventArgs
                    {
                        Result = task.Result
                    }));

                _tasks.Add(task);
            }

            _startTime = DateTime.Now;

            _tasks.ForEach(x => x.Start());

            StartWaitingTask();
        }

        public event EventHandler<TaskEventArgs> TaskFinished;

        public event EventHandler ExecutionFinished;

        public void StartRunning()
        {
            StartRunning(_actionToRun);
        }

        public void Join()
        {
            _resetEvent.WaitOne();
        }

        public void StopRunning()
        {
            _stop = true;
            Join();
            _stopTime = DateTime.Now;
            _status = TaskRunnerStatus.Stopped;
        }

        #endregion

        private void OnTaskFinished(TaskEventArgs e)
        {
            _tasksFinished++;
            _results.Add(e.Result);

            EventHandler<TaskEventArgs> handler = TaskFinished;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnExecutionFinished(EventArgs e = null)
        {
            EventHandler handler = ExecutionFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void StartWaitingTask()
        {
            var waitTask = new Task(() =>
                {
                    _tasks.ForEach(task => task.Wait());

                    _stopTime = DateTime.Now;

                    lock (_lock)
                    {
                        _status = TaskRunnerStatus.Finished;
                    }

                    _resetEvent.Set();

                    OnExecutionFinished();
                });

            waitTask.Start();
        }

        private void InitializeCounters()
        {
            _tasksFinished = 0;
            _totalIterationsFinished = 0;
        }

        private TaskResult TaskRoutine(object action)
        {
            Exception exception = null;
            int localIterationsFinished = 0;
            var actionToInvoke = action as Action;

            if (actionToInvoke == null)
            {
                return new TaskResult
                    {
                        ExecutionFailed = true,
                    };
            }

            TimeSpan timeTaken = _performanceCounter.GetExecutingTime(
                () =>
                    {
                        for (int j = 0; j < IterationsCount; j++)
                        {
                            if (_stop)
                            {
                                return;
                            }

                            try
                            {
                                actionToInvoke.Invoke();
                            }
                            catch (Exception ex)
                            {
                                exception = ex;
                            }
                            finally
                            {
                                Thread.MemoryBarrier();
                                _totalIterationsFinished++;
                                Thread.MemoryBarrier();
                                localIterationsFinished++;
                                Thread.MemoryBarrier();
                            }
                        }
                    }
                );

            var result = new TaskResult
                {
                    ExceptionThrown = exception,
                    WasExceptionThrown = exception != null,
                    IterationsFinished = localIterationsFinished,
                    TimeTaken = timeTaken,
                };

            return result;
        }
    }
}