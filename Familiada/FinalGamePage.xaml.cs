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
    public partial class FinalGamePage : Page
    {
        ServerWindow parent;
        List<int?> pointsList = new List<int?>();

        int nextDataIndex = 0;
        List<KeyValuePair<int, String>> dataToSend = null;

        public FinalGamePage(ServerWindow window)
        {
            InitializeComponent();

            parent = window;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RoundData d = parent.round;

            for (int i = 0; i < d.final.Length; i++)
            {
                Question q = d.final[i];
                (questionStack.Children[i] as Label).Content = q.question;
                foreach (KeyValuePair<int, String> ans in q.answers)
                {
                    (answerStack.Children[i] as ComboBox).Items.Add(ans.ToString());
                }
            }

            pointsList.AddRange(new int?[d.final.Length]);
            pointsList.ForEach(delegate(int? i) { i = 0; });

            parent.client.LoadFinalGame(Math.Max(parent.pointsA, parent.pointsB));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            FinalAnswersWindow answersWindow = new FinalAnswersWindow();
            answersWindow.ShowDialog();

            userStack.Children.Clear();
            foreach (var answer in answersWindow.answers)
            {
                TextBox l = new TextBox();
                l.Text = answer;
                l.Width = 140;
                userStack.Children.Add(l);
            }

            if (userStack.Children.Count < 5)
            {
                for (int i = 5 - userStack.Children.Count; i > 0; i--)
                {
                    TextBox l = new TextBox();
                    l.Width = 140;
                    userStack.Children.Add(l);
                }
            }
        }

        private void answerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            int index = Int32.Parse(c.Name.Substring(8, 1)) - 1;
            pointsList[index] = parent.round.final[index].answers[c.SelectedIndex].Key;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (dataToSend == null)
            {
                dataToSend = new List<KeyValuePair<int, string>>();
                dataToSend.AddRange(new KeyValuePair<int, string>[pointsList.Count]);
                for (int i = 0; i < pointsList.Count; i++)
                {
                    dataToSend[i] = new KeyValuePair<int, string>(
                        pointsList[i].HasValue ? pointsList[i].Value : 0,
                        (userStack.Children[i] as TextBox).Text);
                }
            }
            else
            {
                parent.client.ShowNextFinalAnswer(dataToSend[nextDataIndex++]);
                if (nextDataIndex == dataToSend.Count) button3.IsEnabled = false;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            parent.NextPage(pointsList);
        }
    }
}
