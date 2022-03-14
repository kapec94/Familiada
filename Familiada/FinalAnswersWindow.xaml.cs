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
using System.Timers;

namespace Familiada
{
    public partial class FinalAnswersWindow : Window
    {
        Timer timer = new Timer(1000);

        int timeLeft = 20;
        int answerCount;
        internal List<String> answers = new List<string>();

        public FinalAnswersWindow(int timeoutSeconds = 20, int maxAnswers = 5)
        {
            InitializeComponent();

            timeLeft = timeoutSeconds;
            answerCount = maxAnswers;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timeLeft >= 0)
            {
                this.Dispatcher.Invoke(onTimerElapsed);
            }
            if (timeLeft == -1)
            {
                this.Dispatcher.Invoke(onTimerDone);
            }
        }

        void onTimerElapsed()
        {
            timeLeft -= 1;
            timeLeftLabel.Content = timeLeft;
        }

        void onTimerDone()
        {
            timer.Stop();
            timeLeftLabel.Content = "";
            MessageBox.Show("Koniec czasu!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.Focus();
            timer.Start();
        }

        private bool onAddAnswer(string answerText)
        {
            answers.Add(answerText);
            if (answers.Count >= answerCount)
            {
                return false;
            }
            return true;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            var shouldQuit = e.Key == Key.Escape;
            var shouldAddAnother = false;

            if (e.Key == Key.Enter)
            {
                var answerText = (sender as TextBox).Text;
                shouldAddAnother = onAddAnswer(answerText);
                shouldQuit = !shouldAddAnother;
            }

            if (shouldAddAnother)
            {
                var textBox = new TextBox();
                textBox.Width = 187;
                textBox.KeyDown += textBox_KeyDown;
                stackPanel1.Children.Add(textBox);
                textBox.Focus();
            }

            if (shouldQuit)
            {
                timer.Stop();
                this.Close();
            }
        }

        private void button_Duplicate_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer.PlaySound("zla3");
        }
    }
}
