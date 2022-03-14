using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        ServerWindow server;
        ClientWindow client;
        List<Question> questions;
        int roundTimeSeconds;
        List<int?> pointsList = new List<int?>();
        int initialPoints;
        int answerToSend = 0;
        bool isFirstPlayer;

        internal FinalGamePage(
            ServerWindow serverWnd,
            ClientWindow clientWnd,
            List<Question> questions,
            int initialPoints,
            bool isFirstPlayer,
            int roundTimeSeconds = 20)
        {
            InitializeComponent();

            this.server = serverWnd;
            this.client = clientWnd;
            this.questions = questions;
            this.roundTimeSeconds = roundTimeSeconds;
            this.initialPoints = initialPoints;
            this.isFirstPlayer = isFirstPlayer;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (questions.Count == 0) {
                // We don't have a final. Let's move on.
                server.NextPage(pointsList);
                return;
            }

            // Clear question and answer stacks from content
            foreach (var questionStackChild in questionStack.Children)
            {
                (questionStackChild as Label).Content = "";
            }
            foreach (var answerStackChild in answerStack.Children)
            {
                (answerStackChild as ComboBox).Items.Clear();
            }

            // Fill it again with content
            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                (questionStack.Children[i] as Label).Content = q.question;

                var answerCombo = answerStack.Children[i] as ComboBox;
                foreach (var answer in q.answers)
                {
                    answerCombo.Items.Add(answer.ToString());
                }
                answerCombo.Items.Add("[Zła odpowiedź]");
            }

            pointsList.AddRange(new int?[questions.Count + 1]);
            pointsList.ForEach(delegate(int? i) { i = 0; });

            client.LoadFinalGame(initialPoints);

            if (isFirstPlayer)
            {
                SoundPlayer.PlaySound("przerywnik-final");
            }
            else
            {
                SoundPlayer.PlaySound("przerywnik-final-runda");
            }
        }

        private TextBox buildUserAnswerBox(string text = null)
        {
            var box = new TextBox
            {
                Width = 140,
                Height = 23,
                Margin = new Thickness { Bottom = 5 }
            };
            if (text != null)
            {
                box.Text = text;
            }
            return box;
        }

        private void enterAnswersBtn_Click(object sender, RoutedEventArgs e)
        {
            var answersWindow = new FinalAnswersWindow(roundTimeSeconds);
            answersWindow.ShowDialog();

            userStack.Children.Clear();
            foreach (var answer in answersWindow.answers)
            {
                var box = buildUserAnswerBox(answer);
                userStack.Children.Add(box);
            }

            var questionCount = questions.Count;
            var addedAnswersCount = userStack.Children.Count;

            if (addedAnswersCount < questionCount)
            {
                for (int i = questionCount - addedAnswersCount; i > 0; i--)
                {
                    var box = buildUserAnswerBox();
                    userStack.Children.Add(box);
                }
            }
        }

        private void answerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = sender as ComboBox;
            var index = Int32.Parse(c.Name.Substring(8, 1)) - 1;

            var answers = questions[index].answers;
            var selectedAnswerIndex = c.SelectedIndex;

            if (selectedAnswerIndex >= answers.Count)
            {
                // This is the index of "Wrong answer" answer.
                // We reward 0 points for it and play an error sound.
                pointsList[index] = 0;
            }
            else
            {
                // This is one of the correct answers. Let's look up how many points is it worth.
                pointsList[index] = answers[selectedAnswerIndex].Key;
            }
        }

        private void showResultsBtn_Click(object sender, RoutedEventArgs e)
        {
            var userAnswer = (userStack.Children[answerToSend] as TextBox).Text;
            var answerPoints = pointsList[answerToSend].GetValueOrDefault(0);

            client.ShowNextFinalAnswer(answerPoints, userAnswer);

            answerToSend += 1;
            if (answerToSend >= questions.Count)
            {
                showResultsBtn.IsEnabled = false;
            }

            if (answerPoints > 0)
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

        private void nextPlayerBtn_Click(object sender, RoutedEventArgs e)
        {
            server.NextPage(pointsList);
        }
    }
}
