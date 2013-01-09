using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Familiada
{
    class Question
    {
        public String question;
        public List<KeyValuePair<int, String>> answers;

        public Question(String question)
        {
            this.question = question;
            answers = new List<KeyValuePair<int, String>>();
        }

        public void AddAnswer(String answer, int points)
        {
            answers.Add(new KeyValuePair<int, String>(points, answer));
        }
    }
}
