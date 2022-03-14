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
        private ClientWindow client = null;

        int winnerPoints = 0;

        int currentPageIndex = 0;

        RoundData roundData;

        public ServerWindow()
        {
            InitializeComponent();
            client = new ClientWindow();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var loadPage = new DataSelectPage(this);
            this.MainFrame.Navigate(loadPage);
            client.Show();
            this.Focus();
        }

        internal void NextPage(params Object[] args)
        {
            List<int?> points;

            var nextPageIndex = currentPageIndex + 1;
            Page nextPage;

            switch (nextPageIndex)
            {
                case 1:
                    roundData = RoundData.Load(args[0] as String);
                    nextPage = new NormalGamePage(this, roundData, client);
                    break;

                case 2:
                    winnerPoints = (args[0] as int?).Value;
                    nextPage = new FinalGamePage(
                        this, client, roundData.final, 
                        winnerPoints, true, roundData.finalRoundTimeSeconds);
                    break;

                case 3:
                    points = args[0] as List<int?>;
                    points.ForEach(delegate(int? i) { if (i.HasValue) winnerPoints += i.Value; });
                    nextPage = new FinalGamePage(
                        this, client, roundData.final, 
                        winnerPoints, false, roundData.finalRoundTimeSeconds);
                    break;

                case 4:
                    points = args[0] as List<int?>;
                    points.ForEach(delegate(int? i) { if (i.HasValue) winnerPoints += i.Value; });
                    nextPage = null;
                    break;

                default:
                    return;
            }

            if (nextPage != null)
            {
                MainFrame.Navigate(nextPage);
            }
            else
            {
                client.LoadLastPage(winnerPoints);
            }
            currentPageIndex = nextPageIndex;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
