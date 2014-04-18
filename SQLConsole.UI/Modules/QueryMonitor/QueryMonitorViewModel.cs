using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SQLConsole.BizLogic;
using SQLConsole.BizLogic.DataAccess;
using SQLConsole.BizLogic.TaskRunning;
using SQLConsole.BizLogic.Text;
using SQLConsole.UI.Commands;
using SQLConsole.UI.Types;
using SQLConsole.UI.ViewModels;
using SqlConsole.BizLogic.Exceptions;

namespace SQLConsole.UI.Modules.QueryMonitor
{
    public class QueryMonitorViewModel : ViewModelBase
    {
        private RelayCommand _clearCommand;
        private readonly Dispatcher _dispatcher;
        private TimeSpan _interval;
        private bool _running;
        private RelayCommand _startStopCommand;
        private RelayCommand _newRunCommand;
        private readonly List<TaskTimer> _timers;
        private readonly ISqlClient _sqlClient;
        private string _sqlScript;
        private List<NamedD3DataSource<Point>> _plotCollection;
        private readonly ITextUtils _textUtils;
        private DateTime _runStarted;
        private IList<SqlScript> _scripts;

        public QueryMonitorViewModel(Dispatcher dispatcher, ISqlClient sqlClient, ITextUtils textUtils)
        {
            _sqlClient = sqlClient;
            _dispatcher = dispatcher;
            _textUtils = textUtils;
            _timers = new List<TaskTimer>();
            _plotCollection = new List<NamedD3DataSource<Point>>();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var taskTimer = sender as TaskTimer;
            if (taskTimer != null)
            {
                var plot = taskTimer.TaskParameter as NamedD3DataSource<Point>;
                if (plot != null)
                {
                    var value = _sqlClient.ExecuteScalar<int>(plot.SqlCode);
                    plot.AppendAsync(_dispatcher, new Point((DateTime.Now - _runStarted).TotalSeconds, value));
                    return;
                }
            }

            throw new BizLogicException(ExceptionMessages.TimerTaskError);
        }

        public bool Running
        {
            get { return _running; }
            set
            {
                _running = value;
                NotifyPropertyChanged("Running");
            }
        }

        public double Interval
        {
            get { return _interval.TotalMilliseconds; }
            set
            {
                _interval = TimeSpan.FromMilliseconds(value);
                NotifyPropertyChanged("Interval");
            }
        }

        public List<NamedD3DataSource<Point>> PlotCollection
        {
            get
            {
                return _plotCollection;
            }
            set
            {
                _plotCollection = value;
                NotifyPropertyChanged("PlotCollection");
            }
        }

        public string SqlScript
        {
            get { return _sqlScript; }
            set
            {
                _sqlScript = value;
                NotifyPropertyChanged("SqlScript");
            }
        }

        public ICommand StartStopCommand
        {
            get
            {
                return _startStopCommand ??
                       (_startStopCommand = new RelayCommand(execute => StartStop(), canExecute => true));
            }
        }

        public ICommand NewRunCommand
        {
            get
            {
                return _newRunCommand ??
                       (_newRunCommand = new RelayCommand(execute => NewRun(), canExecute => true));
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ??
                       (_clearCommand = new RelayCommand(execute => ClearPlots(), canExecute => true));
            }
        }

        private void StartStop()
        {
            if (Running)
            {
                StopTimers();
            }
            else
            {
                StartTimers();
            }
        }

        private void ClearPlots()
        {
            foreach (var plot in PlotCollection)
            {
                plot.Collection.Clear();
            }
        }

        private void NewRun()
        {
            _runStarted = DateTime.Now;
            _scripts = _textUtils.GetScripts(SqlScript);
            
            var plotCollection = new List<NamedD3DataSource<Point>>();

            ClearTimers();
            ClearPlots();

            foreach (var script in _scripts)
            {
                var plot = new NamedD3DataSource<Point>
                    {
                        Name = script.Name,
                        SqlCode = script.Code,
                    };

                plot.SetXYMapping(p => p);

                plotCollection.Add(plot);

                var timer = new TaskTimer
                    {
                        AutoReset = true,
                        TaskParameter = plot,
                        Interval = Interval,
                    };

                timer.Elapsed += TimerOnElapsed;

                _timers.Add(timer);
            }

            // must do this here because now we have all plots
            PlotCollection = plotCollection;

            StartTimers();
        }

        private void ClearTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Stop();
                timer.Dispose();
            }

            _timers.Clear();
        }

        private void StopTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Stop();
            }

            Running = false;
        }

        private void StartTimers()
        {
            Running = true;

            foreach (var timer in _timers)
            {
                timer.Start();
            }
        }
    }
}