using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EndlessProxyGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public DataTable Log;
        private EndlessProxy.Proxy proxy;

        public MainWindow()
        {
            InitializeComponent();
            Log = new DataTable();
            Log.Columns.Add("Who");
            Log.Columns.Add("Family");
            Log.Columns.Add("Action");
            Log.Columns.Add("Data");

            for (var i = 0; i < 100; ++i)
            {
                var row = Log.NewRow();
                row[0] = "Blah";
                row[1] = "Blah";
                row[2] = "Blah";
                row[3] = "Blah";
                Log.Rows.Add(row);
            }

            ProxyLog.ItemsSource = Log.DefaultView;
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            proxy = new EndlessProxy.Proxy();
            await proxy.AcceptConnection();
        }
    }
}
