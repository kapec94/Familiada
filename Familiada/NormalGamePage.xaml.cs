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
        ServerWindow parent;

        int answerMode = 0;

        int mistakesA = 0;
        int mistakesB = 0;

        int pointsNeutral = 0;
        int currentQuestion = -1;

        Question question = null;

        public NormalGamePage(ServerWindow window)
        {
            parent = window;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pointsALabel.Content = parent.pointsA;
            pointsBLabel.Content = parent.pointsB;

            pointsSumLabel.Content = pointsNeutral;

            mistakeALabel.Content = mistakeBLabel.Content = String.Empty;

            currentQuestion = 0;
            LoadQuestion(currentQuestion);
            roundNumber.Content = String.Format("Pytanie {0}", currentQuestion + 1);

            parent.client.LoadNormalGame();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (++currentQuestion >= 5)
            {
                parent.NextPage();
                return;
            }

            for (int i = 1; i <= 6; i++)
            {
                (WindowRoot.FindName("ok_" + i) as Button).IsEnabled = true;
                (WindowRoot.FindName("no_" + i) as Button).IsEnabled = true;
            }

            LoadQuestion(currentQuestion);
            roundNumber.Content = String.Format("Pytanie {0}", currentQuestion + 1);

            groupChoose.IsChecked = true;
        }

        private void LoadQuestion(int question)
        {
            foreach (Label l in answersPanel.Children) { l.Content = String.Empty; }

            RoundData d = parent.round;
            Question q = d.normal[question];

            questionLabel.Content = q.question;
            for (int i = 0; i < q.answers.Count; i++)
            {
                (answersPanel.Children[i] as Label).Content = String.Format("{0}", q.answers[i]);
            }

            this.question = q;

            parent.client.LoadQuestion(q);
        }

        private void addPointTo_Click(object sender, RoutedEventArgs e)
        {
            if (sender == addPointToA)
            {
                parent.pointsA += pointsNeutral;
                pointsALabel.Content = parent.pointsA;
            }
            else
            {
                parent.pointsB += pointsNeutral;
                pointsBLabel.Content = parent.pointsB;
            }

            pointsNeutral = 0;
            pointsSumLabel.Content = pointsNeutral;

            parent.client.setPointsA(parent.pointsA);
            parent.client.setPointsB(parent.pointsB);
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            Button b = (sender as Button);
            int number = Int32.Parse((b.Name as String).Substring(3, 1));

            pointsNeutral += question.answers[number - 1].Key;
            pointsSumLabel.Content = pointsNeutral;

            b.IsEnabled = false;
            (WindowRoot.FindName("no_" + number) as Button).IsEnabled = false;

            parent.client.ShowAnswer(number - 1);
            parent.client.SetPointsSum(pointsNeutral);
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            Button b = (sender as Button);
            int number = Int32.Parse((b.Name as String).Substring(3, 1));

            b.IsEnabled = false;
            (WindowRoot.FindName("no_" + number) as Button).IsEnabled = false;

            parent.client.ShowAnswer(number - 1);
        }

        private void mistake_Click(object sender, RoutedEventArgs e)
        {
            int group = (sender == mistakeAButton ? 0 : 1);
            if (group == 0)
            {
                mistakesA++;
                parent.client.AddMistakeA();
            }
            else
            {
                mistakesB++;
                parent.client.AddMistakeB();
            }

            switch (answerMode)
            {
                case 0:
                    if (mistakesA == 1)
                    {
                        mistakeAButton.IsEnabled = false;
                    }
                    if (mistakesB == 1)
                    {
                        mistakeBButton.IsEnabled = false;
                    }
                    break;
                case 1:
                    if (mistakesA == 3)
                    {
                        mistakeAButton.IsEnabled = false;
                    }
                    if (mistakesB == 1)
                    {
                        mistakeBButton.IsEnabled = false;
                    }
                    break;
                case 2:
                    if (mistakesA == 1)
                    {
                        mistakeAButton.IsEnabled = false;
                    }
                    if (mistakesB == 3)
                    {
                        mistakeBButton.IsEnabled = false;
                    }
                    break;
            }

            mistakeALabel.Content = mistakeBLabel.Content = String.Empty;
            for (int i = 0; i < mistakesA; i++) mistakeALabel.Content = (mistakeALabel.Content as String) + 'X';
            for (int i = 0; i < mistakesB; i++) mistakeBLabel.Content = (mistakeBLabel.Content as String) + 'X';
        }

        private void answerMode_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == groupChoose) answerMode = 0;
            else if (sender == groupAAnswer) answerMode = 1;
            else if (sender == groupBAnswer) answerMode = 2;

            mistakeALabel.Content = mistakeBLabel.Content = String.Empty;
            mistakeAButton.IsEnabled = true;
            mistakeBButton.IsEnabled = true;
            mistakesA = mistakesB = 0;

            parent.client.ClearMistakes();
        }
    }
}
