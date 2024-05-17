using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class FAQ : IXmlSerializable
{
    private int _id;
    private string _question;
    private string _answer;

    public FAQ() { }

    public FAQ(int id, string question, string answer)
    {
        _id = id;
        _question = question;
        _answer = answer;
    }

    public string Question { get => _question; set => _question = value; }
    public string Answer { get => _answer; set => _answer = value; }
    public int Id { get => _id; set => _id = value; }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();
        bool isEmptyElement = reader.IsEmptyElement;
        reader.ReadStartElement();
        if (!isEmptyElement)
        {
            _id = int.Parse(reader.ReadElementString("_id"));
            _question = reader.ReadElementString("_question");
            _answer = reader.ReadElementString("_answer");
            reader.ReadEndElement();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("_id", _id.ToString());
        writer.WriteElementString("_question", _question);
        writer.WriteElementString("_answer", _answer);
    }
}
