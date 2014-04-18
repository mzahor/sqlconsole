using System;
using System.Linq;
using SQLConsole.BizLogic.TaskRunning;
using SQLConsole.UI.ViewModels;
using SqlConsole.BizLogic.TaskRunning;

namespace SQLConsole.UI.Modules.StressTool
{
    public class CurrentResultsViewModel : ViewModelBase
    {
        private int _exceptionsCount;
        private int _iterationsFinished;
        private int _iterationsRemaining;
        private int _iterationsTotalCount;
        private TaskRunnerStatus _status;
        private int _tasksFinished;
        private int _tasksRemaining;
        private int _tasksTotalCount;
        private double _totalCompletion;
        private TimeSpan _totalTimeTaken;

        public TaskRunnerStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public int IterationsFinished
        {
            get { return _iterationsFinished; }
            set
            {
                _iterationsFinished = value;
                NotifyPropertyChanged("IterationsFinished");
            }
        }

        public int TasksFinished
        {
            get { return _tasksFinished; }
            set
            {
                _tasksFinished = value;
                NotifyPropertyChanged("TasksFinished");
            }
        }

        public double TotalCompletion
        {
            get { return _totalCompletion; }
            set
            {
                _totalCompletion = value;
                NotifyPropertyChanged("TotalCompletion");
            }
        }

        public int TasksRemaining
        {
            get { return _tasksRemaining; }
            set
            {
                _tasksRemaining = value;
                NotifyPropertyChanged("TasksRemaining");
            }
        }

        public int IterationsRemaining
        {
            get { return _iterationsRemaining; }
            set
            {
                _iterationsRemaining = value;
                NotifyPropertyChanged("IterationsRemaining");
            }
        }

        public int TasksTotalCount
        {
            get { return _tasksTotalCount; }
            set
            {
                _tasksTotalCount = value;
                NotifyPropertyChanged("TasksTotalCount");
            }
        }

        public int IterationsTotalCount
        {
            get { return _iterationsTotalCount; }
            set
            {
                _iterationsTotalCount = value;
                NotifyPropertyChanged("IterationsTotalCount");
            }
        }

        public int ExceptionsCount
        {
            get { return _exceptionsCount; }
            set
            {
                _exceptionsCount = value;
                NotifyPropertyChanged("ExceptionsCount");
            }
        }

        public TimeSpan TotalTimeTaken
        {
            get { return _totalTimeTaken; }
            set
            {
                _totalTimeTaken = value;
                NotifyPropertyChanged("TotalTimeTaken");
            }
        }

        public void UpdateValues(ITaskRunner taskRunner)
        {
            Status = taskRunner.Status;
            ExceptionsCount = taskRunner.Results.Count(x => x.WasExceptionThrown);

            IterationsTotalCount = taskRunner.IterationsCount;
            IterationsFinished = taskRunner.TotalIterationsFinished;
            IterationsRemaining = taskRunner.TotalIterationsRemaining;

            TasksTotalCount = taskRunner.TasksCount;
            TasksFinished = taskRunner.TasksFinished;
            TasksRemaining = taskRunner.TasksRemaining;

            TotalCompletion = taskRunner.TotalProgress;

            TotalTimeTaken = taskRunner.TotalTimeTaken;
        }
    }
}