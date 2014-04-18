using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace SQLConsole.UI.Types
{
    public class NamedD3DataSource<T> : ObservableDataSource<T>
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _sqlCode = string.Empty;
        public string SqlCode
        {
            get { return _sqlCode; }
            set { _sqlCode = value; }
        }
    }
}