using System.Data.SqlClient;
using SQLConsole.UI.ViewModels;
using SqlConsole.BizLogic.Configuration;

namespace SQLConsole.UI.Modules.Connection
{
    public class SqlConnectionViewModel : ViewModelBase
    {
        private readonly SqlConnectionStringBuilder _builder;
        private readonly IUserSettings _settings;

        public SqlConnectionViewModel(IUserSettings settings)
        {
            _settings = settings;
            _builder = new SqlConnectionStringBuilder(_settings.ConnectionString);
        }

        public string ConnectionString
        {
            get { return _builder.ConnectionString; }
            set
            {
                _builder.ConnectionString = value;
                NotifyPropertyChanged("ConnectionString");
                UpdateSettings();
            }
        }

        public bool IntegratedSecurity
        {
            get { return _builder.IntegratedSecurity; }
            set
            {
                _builder.IntegratedSecurity = value;
                NotifyPropertyChanged("IntegratedSecurity");
                NotifyPropertyChanged("ConnectionString");
                UpdateSettings();
            }
        }

        public string Server
        {
            get { return _builder.DataSource; }
            set
            {
                _builder.DataSource = value;
                NotifyPropertyChanged("Server");
                NotifyPropertyChanged("ConnectionString");
                UpdateSettings();
            }
        }

        public string Database
        {
            get { return _builder.InitialCatalog; }
            set
            {
                _builder.InitialCatalog = value;
                NotifyPropertyChanged("Database");
                NotifyPropertyChanged("ConnectionString");
                UpdateSettings();
            }
        }

        public string Login
        {
            get { return _builder.UserID; }
            set
            {
                _builder.UserID = value;
                NotifyPropertyChanged("Login");
                NotifyPropertyChanged("ConnectionString");
                UpdateSettings();
            }
        }

        public string Password
        {
            get { return _builder.Password; }
            set
            {
                _builder.Password = value;
                NotifyPropertyChanged("Password");
                NotifyPropertyChanged("ConnectionString");
                UpdateSettings();
            }
        }

        private void UpdateSettings()
        {
            _settings.ConnectionString = _builder.ConnectionString;
        }
    }
}