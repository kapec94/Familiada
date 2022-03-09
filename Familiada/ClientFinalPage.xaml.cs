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
    /// <summary>
    /// Interaction logic for ClientFinalPage.xaml
    /// </summary>
    public partial class ClientFinalPage : Page
    {
        public ClientFinalPage()
        {
            InitializeComponent();
        }

        public void SetScore(int score)
        {
            scoreLabel.Content = score;
        }

        public void SetSumScore(int score)
        {
            sumLabel.Content = String.Format("SUMA {0}", score);
        }

        public void ShowAnswer(int page, int index, String answer, int points)
        {
            if (page == 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(answer);
                if (sb.Length < 10)
                {
                    for (int i = 10 - sb.Length; i > 0; i--)
                    {
                        sb.Append(' ');
                    }
                }
                sb.Append(' ');
                sb.AppendFormat("{0:00} ", points);

                (answerStackA.Children[index] as Label).Content = sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" {0:00} ", points);
                sb.Append(answer.PadLeft(10));

                (answerStackB.Children[index] as Label).Content = sb.ToString();
            }
        }
    }
}
