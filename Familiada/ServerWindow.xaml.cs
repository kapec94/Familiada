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
using System.Windows.Shapes;
using System.IO;

namespace Familiada
{
    public partial class ServerWindow : Window
    {
        internal ClientWindow client = null;

        internal RoundData round = null;

        internal int pointsA = 0;
        internal int pointsB = 0;

        int winnerPoints = 0;

        int currentPage = 0;
        Page[] pages = null;

        public ServerWindow()
        {
            InitializeComponent();

            client = new ClientWindow();

            pages = new Page[5];
            pages[0] = new DataSelectPage(this);
            pages[1] = new NormalGamePage(this);
            pages[2] = new FinalGamePage(this);
            pages[3] = new FinalGamePage(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainFrame.Navigate(pages[0]);
            client.Show();
        }

        internal void NextPage(params Object[] args)
        {
            List<int?> points;
            switch (currentPage)
            {
                case 0:
                    round = RoundData.Load(args[0] as String);
                    break;

                case 1:
                    winnerPoints = Math.Max(pointsA, pointsB);
                    break;

                case 2:
                    points = args[0] as List<int?>;
                    points.ForEach(delegate(int? i) { if (i.HasValue) winnerPoints += i.Value; });
                    break;

                case 3:
                    points = args[0] as List<int?>;
                    points.ForEach(delegate(int? i) { if (i.HasValue) winnerPoints += i.Value; });
                    this.client.LoadLastPage();
                    return;
            }

            MainFrame.Navigate(pages[++currentPage]);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
