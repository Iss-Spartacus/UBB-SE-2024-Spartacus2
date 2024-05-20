using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class FAQ : IXmlSerializable
{
    private int id;
    private string question;
    private string answer;

    public FAQ()
    {
    }

    public FAQ(int id, string question, string answer)
    {
        this.id = id;
        this.question = question;
        this.answer = answer;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Question
    {
        get => question;
        set => question = value;
    }

    public string Answer
    {
        get => answer;
        set => answer = value;
    }

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
            id = int.Parse(reader.ReadElementString(nameof(Id)));
            question = reader.ReadElementString(nameof(Question));
            answer = reader.ReadElementString(nameof(Answer));
            reader.ReadEndElement();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString(nameof(Id), id.ToString());
        writer.WriteElementString(nameof(Question), question);
        writer.WriteElementString(nameof(Answer), answer);
    }
}
