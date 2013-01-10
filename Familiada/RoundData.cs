using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Familiada
{
    class RoundData
    {
        public Question[] normal;
        public Question[] final;

        public RoundData()
        {
            normal = new Question[5];
            final = new Question[5];
        }

        static public void Save(RoundData round, string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings { NewLineHandling = NewLineHandling.Entitize, Indent = true, CloseOutput = true };
            XmlWriter writer = XmlWriter.Create(path, settings);

            writer.WriteStartDocument(false);
            writer.WriteStartElement("RoundData");

            writer.WriteStartElement("NormalMode");
            foreach (var q in round.normal)
            {
                writer.WriteStartElement("Question");
                writer.WriteAttributeString("Title", q.question);

                foreach (var pair in q.answers)
                {
                    writer.WriteStartElement("Answer");
                    writer.WriteAttributeString("Points", pair.Key.ToString());
                    writer.WriteString(pair.Value);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("FinalMode");
            foreach (var q in round.final)
            {
                writer.WriteStartElement("Question");
                writer.WriteAttributeString("Title", q.question);

                foreach (var pair in q.answers)
                {
                    writer.WriteStartElement("Answer");
                    writer.WriteAttributeString("Points", pair.Key.ToString());
                    writer.WriteString(pair.Value);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }

        static public RoundData Load(string path)
        {
            RoundData d = new RoundData();

            XmlReaderSettings settings = new XmlReaderSettings { CloseInput = true, IgnoreComments = true };
            XmlReader reader = XmlReader.Create(path, settings);

            // TODO: For the God's sake, could someone rewrite it so it's not the same things twice? DRY, man, DRY.

            reader.ReadToFollowing("NormalMode");
            reader.ReadToDescendant("Question");
            int i = 0;
            do
            {
                reader.MoveToAttribute("Title");
                string questionString = reader.ReadContentAsString();

                Question q = new Question(questionString);

                reader.MoveToElement();
                reader.ReadToDescendant("Answer");
                do
                {
                    reader.MoveToAttribute("Points");
                    int points = reader.ReadContentAsInt();
                    reader.MoveToElement();
                    string answer = reader.ReadElementContentAsString();

                    q.AddAnswer(answer, points);
                } while (reader.ReadToNextSibling("Answer"));

                d.normal[i++] = q;
            } while (reader.ReadToNextSibling("Question"));

            reader.ReadToFollowing("FinalMode");
            reader.ReadToDescendant("Question");
            i = 0;
            do
            {
                reader.MoveToAttribute("Title");
                string questionString = reader.ReadContentAsString();

                Question q = new Question(questionString);

                reader.MoveToElement();
                reader.ReadToDescendant("Answer");
                do
                {
                    reader.MoveToAttribute("Points");
                    int points = reader.ReadContentAsInt();
                    reader.MoveToElement();
                    string answer = reader.ReadElementContentAsString();

                    q.AddAnswer(answer, points);
                } while (reader.ReadToNextSibling("Answer"));

                d.final[i++] = q;
            } while (reader.ReadToNextSibling("Question"));

            return d;
        }
    }
}
