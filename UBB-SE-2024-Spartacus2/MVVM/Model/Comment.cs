using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Comment : IXmlSerializable
{
    private int id;
    private string username;
    private string content;
    private DateTime dateOfCreation;
    private DateTime dateOfUpdate;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Username
    {
        get => username;
        set => username = value;
    }

    public string Content
    {
        get => content;
        set => content = value;
    }

    public DateTime DateOfCreation
    {
        private get => dateOfCreation;
        set
        {
            dateOfCreation = value;
            dateOfUpdate = dateOfCreation;
        }
    }

    public DateTime DateOfUpdate
    {
        get => dateOfUpdate;
        set => dateOfUpdate = value;
    }

    public Comment()
    {
    }

    public Comment(int id, string username, string content, DateTime creation)
    {
        this.id = id;
        this.username = username;
        this.content = content;
        this.dateOfCreation = creation;
        this.dateOfUpdate = creation;
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        if (reader.IsEmptyElement)
        {
            return;
        }

        reader.ReadStartElement("Comment");

        id = int.Parse(reader.ReadElementString(nameof(Id)));
        username = reader.ReadElementString(nameof(Username));
        content = reader.ReadElementString(nameof(Content));
        dateOfCreation = DateTime.ParseExact(reader.ReadElementString(nameof(DateOfCreation)), "dd-MM-yyyy HH:mm", null);
        dateOfUpdate = DateTime.ParseExact(reader.ReadElementString(nameof(DateOfUpdate)), "dd-MM-yyyy HH:mm", null);

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString(nameof(Id), Id.ToString());
        writer.WriteElementString(nameof(Username), Username);
        writer.WriteElementString(nameof(Content), Content);
        writer.WriteElementString(nameof(DateOfCreation), DateOfCreation.ToString("dd-MM-yyyy HH:mm"));
        writer.WriteElementString(nameof(DateOfUpdate), DateOfUpdate.ToString("dd-MM-yyyy HH:mm"));
    }
}
