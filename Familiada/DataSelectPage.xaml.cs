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

namespace Familiada
{
    /// <summary>
    /// Interaction logic for DataSelect.xaml
    /// </summary>
    public partial class DataSelectPage : Page
    {
        ServerWindow parent;

        public DataSelectPage(ServerWindow window)
        {
            InitializeComponent();

            parent = window;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int set = 0;
            if (sender == button1) set = 1;
            else if (sender == button2) set = 2;
            else if (sender == button3) set = 3;

            parent.NextPage(set);
        }
    }
}
