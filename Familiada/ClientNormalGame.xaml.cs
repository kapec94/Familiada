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
using System.Windows.Markup;
using System.IO;
using System.Xml;

namespace Familiada
{
    /// <summary>
    /// Interaction logic for ClientNormalGame.xaml
    /// </summary>
    public partial class ClientNormalGame : Page
    {
        static String mistakeXaml = null;
        String emptyLine = "{0}. ..........................  0";

        static ClientNormalGame() 
        {
            Label mistakeX = new Label();
            mistakeX.Content = 'X';
            mistakeX.FontFamily = new FontFamily("Consolas");
            mistakeX.Foreground = Brushes.Yellow;
            mistakeX.FontSize = 64;

            mistakeXaml = XamlWriter.Save(mistakeX);
        }

        public ClientNormalGame()
        {
            InitializeComponent();

            /*
            Label mistakeX = new Label();
            mistakeX.Content = 'X';
            mistakeX.FontFamily = new FontFamily("Consolas");
            mistakeX.Foreground = Brushes.Yellow;
            mistakeX.FontSize = 64;

            mistakeXaml = XamlWriter.Save(mistakeX);
             */
        }

        public void ClearMistakesFor(int team)
        {
            if (team == 0) mistakesStackA.Children.Clear();
            else mistakesStackB.Children.Clear();
        }

        public void AddMistakeFor(int team)
        {
            StringReader stringReader = new StringReader(mistakeXaml);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);
            UIElement o = (UIElement)XamlReader.Load(xmlTextReader);

            if (team == 0) mistakesStackA.Children.Add(o);
            else mistakesStackB.Children.Add(o);
        }

        public void ClearAnswers(int answersCount)
        {
            int i = 1;
            foreach (Label l in answerStack.Children)
            {
                l.Content = (answersCount-- > 0 ? String.Format(emptyLine, i++) : String.Empty);
            }
        }

        public void SetAnswer(int index, String answer, int points)
        {
            StringBuilder line = new StringBuilder();
            line.Append(index + 1)
                .Append(". ")
                .Append(answer);

            if (line.Length < 30)
            {
                for (int i = 30 - line.Length; i > 0; i--)
                {
                    line.Append(' ');
                }
            }
            line.AppendFormat("{0:00}", points);

            (answerStack.Children[index] as Label).Content = line.ToString();
        }

        public void UpdatePointsA(int points)
        {
            pointsLabelA.Content = points;
        }

        public void UpdatePointsB(int points)
        {
            pointsLabelB.Content = points;
        }

        public void UpdatePointsSum(int points)
        {
            PointsSum.Content = points;
        }
    }
}
