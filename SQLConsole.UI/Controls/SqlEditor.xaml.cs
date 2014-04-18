using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLConsole.UI.Controls
{
    /// <summary>
    /// Interaction logic for SqlEditor.xaml
    /// </summary>
    public partial class SqlEditor : UserControl
    {
        public SqlEditor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (string), typeof (SqlEditor));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private void SqlScript_TextChanged(object sender, EventArgs e)
        {
            Text = SqlScript.Text;
        }
    }
}
