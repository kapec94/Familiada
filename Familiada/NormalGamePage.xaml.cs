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
    public partial class NormalGamePage : Page
    {
        int mistakesA = 0;
        int mistakesB = 0;

        int pointsNeutral = 0;
        int currentQuestion = -1;

        int pointsA = 0;
        int pointsB = 0;

        RoundData roundData;
        ServerWindow server;
        ClientWindow client;

        Question question = null;

        internal NormalGamePage(ServerWindow serverWnd, RoundData rd, ClientWindow clientWnd)
        {
            InitializeComponent();
            roundData = rd;
            server = serverWnd;
            client = clientWnd;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pointsALabel.Content = pointsA;
            pointsBLabel.Content = pointsB;
            pointsSumLabel.Content = pointsNeutral;
            mistakeALabel.Content = mistakeBLabel.Content = String.Empty;

            currentQuestion = 0;
            LoadQuestion(currentQuestion);
            roundNumber.Content = String.Format("Pytanie {0}", currentQuestion + 1);

            client.LoadNormalGame();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (++currentQuestion >= roundData.normal.Count)
            {
                var winnerPoints = Math.Max(pointsA, pointsB);
                server.NextPage(winnerPoints);
                return;
            }

            for (int i = 1; i <= 6; i++)
            {
                (WindowRoot.FindName("ok_" + i) as Button).IsEnabled = true;
                (WindowRoot.FindName("no_" + i) as Button).IsEnabled = true;
            }

            LoadQuestion(currentQuestion);
            roundNumber.Content = String.Format("Pytanie {0}", currentQuestion + 1);
        }

        private void ClearMistakes()
        {
            this.mistakesA = this.mistakesB = 0;
            this.mistakeALabel.Content = this.mistakeBLabel.Content = String.Empty;

            client.ClearMistakes();
        }

        private void LoadQuestion(int question)
        {
            this.ClearMistakes();
            foreach (Label l in answersPanel.Children) { l.Content = String.Empty; }

            Question q = roundData.normal[question];

            questionLabel.Content = q.question;
            for (int i = 0; i < q.answers.Count; i++)
            {
                (answersPanel.Children[i] as Label).Content = String.Format("{0}", q.answers[i]);
            }

            this.question = q;
            client.LoadQuestion(q);

            SoundPlayer.PlaySound("przerywnik-normal");
        }

        private void addPointTo_Click(object sender, RoutedEventArgs e)
        {
            if (sender == addPointToA)
            {
                pointsA += pointsNeutral;
                pointsALabel.Content = pointsA;
            }
            else
            {
                pointsB += pointsNeutral;
                pointsBLabel.Content = pointsB;
            }

            pointsNeutral = 0;
            pointsSumLabel.Content = pointsNeutral;

            client.setPointsA(pointsA);
            client.setPointsB(pointsB);
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            // We retrieve clicked button's index from its XAML name. Nice.
            var b = (sender as Button);
            var number = Int32.Parse((b.Name as String).Substring(3, 1));
            var index = number - 1;

            // It's possible that we don't have an answer next to this button.
            if (index >= question.answers.Count) {
                // We don't. Let's bail.
                b.IsEnabled = false;
                return;
            }

            // It looks like we have an answer here. Let's continue.
            pointsNeutral += question.answers[number - 1].Key;
            pointsSumLabel.Content = pointsNeutral;

            b.IsEnabled = false;
            (WindowRoot.FindName("no_" + number) as Button).IsEnabled = false;

            client.ShowAnswer(number - 1);
            client.SetPointsSum(pointsNeutral);

            SoundPlayer.PlaySound("dobra1");
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            var b = (sender as Button);
            var number = Int32.Parse((b.Name as String).Substring(3, 1));
            var index = number - 1;

            b.IsEnabled = false;
            (WindowRoot.FindName("no_" + number) as Button).IsEnabled = false;

            client.ShowAnswer(index);

            SoundPlayer.PlaySound("dobra1");
        }

        private void mistake_Click(object sender, RoutedEventArgs e)
        {
            int group = (sender == mistakeAButton ? 0 : 1);
            if (group == 0)
            {
                mistakesA++;
                client.AddMistakeA();
            }
            else
            {
                mistakesB++;
                client.AddMistakeB();
            }

            mistakeALabel.Content = mistakeBLabel.Content = String.Empty;
            for (int i = 0; i < mistakesA; i++) mistakeALabel.Content = (mistakeALabel.Content as String) + 'X';
            for (int i = 0; i < mistakesB; i++) mistakeBLabel.Content = (mistakeBLabel.Content as String) + 'X';

            SoundPlayer.PlaySound("zla3");
        }

        private void mistake_Clear(object sender, RoutedEventArgs e)
        {
            ClearMistakes();
        }
    }
}
