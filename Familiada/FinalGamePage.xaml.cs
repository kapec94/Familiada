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

            if (d.final.Count == 0) {
                // We don't have a final. Let's move on.
                parent.NextPage(pointsList);
                return;
            }

            for (int i = 0; i < d.final.Count; i++)
            {
                Question q = d.final[i];
                (questionStack.Children[i] as Label).Content = q.question;

                var answerCombo = answerStack.Children[i] as ComboBox;
                foreach (KeyValuePair<int, String> ans in q.answers)
                {
                    answerCombo.Items.Add(ans.ToString());
                }
                answerCombo.Items.Add("[Zła odpowiedź]");
            }

            pointsList.AddRange(new int?[d.final.Count + 1]);
            pointsList.ForEach(delegate(int? i) { i = 0; });

            parent.client.LoadFinalGame(Math.Max(parent.pointsA, parent.pointsB));

            SoundPlayer.PlaySound("przerywnik-final");
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
                l.Height = 23;
                l.Margin = new Thickness { Bottom = 5 };
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

            var finalAnswers = parent.round.final[index].answers;
            var selectedAnswerIndex = c.SelectedIndex;

            if (selectedAnswerIndex >= finalAnswers.Count)
            {
                // This is the index of "Wrong answer" answer.
                // We reward 0 points for it and play an error sound.
                pointsList[index] = 0;
            }
            else
            {
                // This is one of the correct answers. Let's look up how many points is it worth.
                pointsList[index] = finalAnswers[selectedAnswerIndex].Key;
            }
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
                var nextDataToSend = dataToSend[nextDataIndex++];
                parent.client.ShowNextFinalAnswer(nextDataToSend);
                if (nextDataIndex == dataToSend.Count) button3.IsEnabled = false;

                if (nextDataToSend.Key > 0)
                {
                    // The player gave a correct answer and will receive points for it. 
                    SoundPlayer.PlaySound("dobra1");
                }
                else
                {
                    // The answer was incorrect. The player receives 0 points for it.
                    SoundPlayer.PlaySound("zla3");
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            parent.NextPage(pointsList);
            SoundPlayer.PlaySound("przerywnik-final-runda");
        }
    }
}
