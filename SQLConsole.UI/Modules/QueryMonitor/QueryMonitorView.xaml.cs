using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay;
using SQLConsole.UI.Types;

namespace SQLConsole.UI.Modules.QueryMonitor
{
    /// <summary>
    /// Interaction logic for QueryMonitorView.xaml
    /// </summary>
    public partial class QueryMonitorView : UserControl
    {
        public static readonly DependencyProperty PlotCollectionProperty =
            DependencyProperty.Register("PlotCollection", typeof(List<NamedD3DataSource<Point>>),
                                        typeof (QueryMonitorView), new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var view = dependencyObject as QueryMonitorView;
            var newValue = dependencyPropertyChangedEventArgs.NewValue as List<NamedD3DataSource<Point>>;

            if (newValue == null || view == null)
            {
                return;
            }

            view.plotter.Children.RemoveAll(typeof(LineGraph));

            foreach (NamedD3DataSource<Point> t in newValue)
            {
                view.plotter.AddLineGraph(t, 2, t.Name);
            }
        }

        public QueryMonitorView()
        {
            InitializeComponent();
        }

        public List<NamedD3DataSource<Point>> PlotCollection
        {
            get { return (List<NamedD3DataSource<Point>>)GetValue(PlotCollectionProperty); }
            set
            {
                SetValue(PlotCollectionProperty, value);
            }
        }
    }
}