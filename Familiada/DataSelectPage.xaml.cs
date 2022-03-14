using System;
using System.Collections.Generic;
using System.IO;
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

namespace Familiada
{
    /// <summary>
    /// Interaction logic for DataSelect.xaml
    /// </summary>
    public partial class DataSelectPage : Page
    {
        ServerWindow server;

        public DataSelectPage(ServerWindow serverWnd)
        {
            InitializeComponent();
            server = serverWnd;
        }

        /*
        private void button_Click(object sender, RoutedEventArgs e)
        {
            int set = 0;
            if (sender == button1) set = 1;
            else if (sender == button2) set = 2;
            else if (sender == button3) set = 3;

            parent.NextPage(set);
        }
        */

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(".");
            foreach (var xmlFile in dir.GetFiles("*.xml"))
            {
                listView1.Items.Add(xmlFile.Name);
            }
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button1.IsEnabled = (listView1.SelectedIndex != -1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            server.NextPage(listView1.SelectedItem as String);
        }
    }
}
