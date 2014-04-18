using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using SQLConsole.BizLogic.DataAccess;
using SQLConsole.UI.Commands;
using SQLConsole.UI.ViewModels;
using SqlConsole.BizLogic.IoC;
using SqlConsole.BizLogic.TaskRunning;

namespace SQLConsole.UI.Modules.StressTool
{
    public class StressToolViewModel : ViewModelBase
    {
        private readonly Dispatcher _dispatcher;
        private readonly ISqlClient _sqlClient;
        private readonly ITaskRunner _taskRunner;
        private CurrentResultsViewModel _currentResults ;
        private DateTime _startTime;
        private readonly DispatcherTimer _resultsUpdateTimer;
        private const double UpdateResultsInterval = 0.5;

        public StressToolViewModel(Dispatcher dispatcher, ISqlClient sqlClient)
        {
            _dispatcher = dispatcher;

            _resultsUpdateTimer = new DispatcherTimer(DispatcherPriority.Normal, dispatcher)
                {
                    Interval = TimeSpan.FromSeconds(UpdateResultsInterval)
                };

            _resultsUpdateTimer.Tick += (sender, args) => UpdateResults();

            _currentResults = new CurrentResultsViewModel();

            _sqlClient = sqlClient;

            _taskRunner = ObjectFactory.GetTaskRunner();
            _taskRunner.TasksCount = 1;
            _taskRunner.IterationsCount = 1;
            _taskRunner.ActionToRun = () => _sqlClient.RunSql(_sqlScript);
            _taskRunner.ExecutionFinished += TaskRunnerOnExecutionFinished;
        }

        private void UpdateResults()
        {
            _currentResults.UpdateValues(_taskRunner);
        }

        private void TaskRunnerOnExecutionFinished(object sender, EventArgs eventArgs)
        {
            _dispatcher.Invoke((Action) (
                                            () =>
                                                {
                                                    Running = false;
                                                }), DispatcherPriority.DataBind);
        }

        private void Start()
        {
            Running = true;
            _startTime = DateTime.Now;
            _taskRunner.StartRunning();
            UpdateResults();
            _resultsUpdateTimer.Start();
        }

        private void Stop()
        {
            _taskRunner.StopRunning();
            Running = false;
            _resultsUpdateTimer.Stop();
            UpdateResults();
        }

        #region UI related

        private bool _running;

        private string _sqlScript;
        private RelayCommand _startCommand;
        private RelayCommand _stopCommand;

        public bool Running
        {
            get { return _running; }
            set
            {
                _running = value;
                NotifyPropertyChanged("Running");
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SqlScript
        {
            get { return _sqlScript; }
            set
            {
                _sqlScript = value;
                NotifyPropertyChanged("SqlScript");
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public int ThreadCount
        {
            get { return _taskRunner.TasksCount; }
            set
            {
                _taskRunner.TasksCount = value;
                NotifyPropertyChanged("TasksCount");
            }
        }

        public CurrentResultsViewModel CurrentResults
        {
            get { return _currentResults; }
            set
            {
                _currentResults = value;
                NotifyPropertyChanged("CurrentResults");
            }
        }

        public int TimesToRun
        {
            get { return _taskRunner.IterationsCount; }
            set
            {
                _taskRunner.IterationsCount = value;
                NotifyPropertyChanged("IterationsCount");
            }
        }

        public ICommand StartCommand
        {
            get
            {
                return _startCommand ?? (_startCommand = new RelayCommand(execute => Start(),
                                                                          canExecute =>
                                                                          !string.IsNullOrEmpty(SqlScript) && !Running));
            }
        }

        public ICommand StopCommand
        {
            get { return _stopCommand ?? (_stopCommand = new RelayCommand(execute => Stop(), canExecute => Running)); }
        }

        #endregion
    }
}