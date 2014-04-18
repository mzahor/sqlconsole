using System.Windows;
using SQLConsole.BizLogic.DataAccess;
using SQLConsole.BizLogic.Text;
using SQLConsole.UI.Modules.Connection;
using SQLConsole.UI.Modules.QueryMonitor;
using SQLConsole.UI.Modules.StressTool;
using SQLConsole.UI.ViewModels;
using SqlConsole.BizLogic.Configuration;
using SqlConsole.BizLogic.IoC;

namespace SQLConsole.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IUserSettings userSettings = ObjectFactory.GetUserSettings();
            var connectionViewModel = new SqlConnectionViewModel(userSettings);

            ISqlClient sqlClientForSqlRunner = ObjectFactory.GetSqlClient();
            var sqlRunnerViewModel = new StressToolViewModel(Dispatcher, sqlClientForSqlRunner);

            ITextUtils textUtils = ObjectFactory.GetTextUtils();
            ISqlClient sqlClientForQueueMonitor = ObjectFactory.GetSqlClient();
            var queueMonitorViewModel = new QueryMonitorViewModel(Dispatcher, sqlClientForQueueMonitor, textUtils);

            connectionTab.DataContext = connectionViewModel;
            stressToolTab.DataContext = sqlRunnerViewModel;
            queueMonitorTab.DataContext = queueMonitorViewModel;
        }
    }
}