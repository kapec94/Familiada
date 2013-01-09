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

        internal List<String> answers = new List<string>();

        public FinalAnswersWindow()
        {
            InitializeComponent();

            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate() { timeLeftLabel.Content = timeLeft--; }));
            if (timeLeft == -1) this.Dispatcher.Invoke(new Action(delegate() { this.Close(); }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.Focus();
            timer.Start();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                answers.Add((sender as TextBox).Text);
                var textBox = new TextBox();
                textBox.Width = 187;
                textBox.KeyDown += textBox_KeyDown;
                stackPanel1.Children.Add(textBox);
                textBox.Focus();
            }
            else if (e.Key == Key.Escape)
            {
                answers.Add((sender as TextBox).Text);
                timer.Stop();
                this.Close();
            }
        }
    }
}
