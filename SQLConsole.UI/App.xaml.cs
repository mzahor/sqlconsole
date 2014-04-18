using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace SQLConsole.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            LoadResources();
        }

        private void LoadResources()
        {
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("SQLConsole.UI.Resources.SQLHighlighting.xshd"))
            {
                using (var reader = new XmlTextReader(s))
                {
                    Resources.Add("TSQLSyntax", HighlightingLoader.Load(reader, HighlightingManager.Instance));
                }
            }
        }
    }
}
