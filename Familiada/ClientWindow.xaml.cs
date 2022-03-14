using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    public partial class ClientWindow : Window
    {
        private Point previousLocation = new Point();
        private Size previousSize = new Size();

        private Page mainPage = new TitlePage();
        private ClientNormalGame normalPage = new ClientNormalGame();
        private ClientFinalPage finalPage = new ClientFinalPage();
        private Question question;

        public ClientWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.frame.Navigate(mainPage);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (this.IsFullscreen)
                {
                    this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                    this.Topmost = true;
                    this.WindowState = System.Windows.WindowState.Normal;

                    this.Left = previousLocation.X;
                    this.Top = previousLocation.Y;

                    this.Width = previousSize.Width;
                    this.Height = previousSize.Height;
                }
                else
                {
                    previousLocation.X = this.Left;
                    previousLocation.Y = this.Top;

                    previousSize.Width = this.Width;
                    previousSize.Height = this.Height;

                    this.WindowStyle = System.Windows.WindowStyle.None;
                    this.Topmost = false;
                    this.WindowState = System.Windows.WindowState.Maximized;
                }
            }
        }

        private bool IsFullscreen
        {
            get { return this.WindowState == System.Windows.WindowState.Maximized && this.WindowStyle == System.Windows.WindowStyle.None; }
        }

        internal void LoadNormalGame()
        {
            this.frame.Navigate(normalPage);
        }

        internal void LoadQuestion(Question q)
        {
            this.question = q;
            normalPage.ClearAnswers(q.answers.Count);
        }

        internal void ShowAnswer(int index)
        {
            if (index < question.answers.Count) {
                normalPage.SetAnswer(index, question.answers[index].Value, question.answers[index].Key);
            }
        }

        internal void SetPointsSum(int pointsNeutral)
        {
            normalPage.UpdatePointsSum(pointsNeutral);
        }

        internal void AddMistakeB()
        {
            normalPage.AddMistakeFor(1);
        }

        internal void AddMistakeA()
        {
            normalPage.AddMistakeFor(0);
        }

        internal void ClearMistakes()
        {
            normalPage.ClearMistakesFor(0);
            normalPage.ClearMistakesFor(1);
        }

        internal void setPointsA(int p)
        {
            normalPage.UpdatePointsA(p);
            normalPage.UpdatePointsSum(0);
        }

        internal void setPointsB(int p)
        {
            normalPage.UpdatePointsB(p);
            normalPage.UpdatePointsSum(0);
        }

        int a = 0;
        int p = 0;
        int points = 0;
        int pointsAll = 0;
        internal void LoadFinalGame(int score)
        {
            this.frame.Navigate(finalPage);
            finalPage.SetScore(score);
            pointsAll = score;
        }

        internal void ShowNextFinalAnswer(int answerPoints, string answer)
        {
            finalPage.ShowAnswer(p, a++, answer, answerPoints);
            if (a == 5)
            {
                a = 0;
                p = 1;
            }
            points += answerPoints;
            pointsAll += answerPoints;
            finalPage.SetSumScore(points);
            finalPage.SetScore(pointsAll);
        }

        internal void LoadLastPage(int winnerPoints)
        {
            this.frame.Navigate(new ClientLastPage(winnerPoints));
        }
    }
}
