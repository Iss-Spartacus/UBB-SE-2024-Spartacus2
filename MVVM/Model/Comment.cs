using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Comment : IXmlSerializable
{
    private int _id;
    private string _username;
    private string _content;
    private DateTime _dateOfCreation;
    private DateTime _dateOfUpdate;

    public int Id { get => _id; set => _id = value; }
    public string Username { get => _username; set => _username = value;  }
    public string Content { get => _content; set => _content = value; }
    public DateTime DateOfCreation 
    { 
        private get => _dateOfCreation; 
        set 
        { _dateOfCreation = value; 
            _dateOfUpdate = _dateOfCreation; 
        } 
    }
    public DateTime DateOfUpdate { get => _dateOfUpdate; set => _dateOfUpdate = value; }

    public Comment()
    {

    }

    public Comment(int id, string username, string content, DateTime creation)
    {
        Id = id;
        Username = username;
        Content = content;
        DateOfCreation = creation;
        DateOfUpdate = creation;
    }

    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToContent();

        // TODO: this if is experimental. It tries to not read the comment if it is empty. Did not test it.
        if (reader.IsEmptyElement)
        {
            return;
        }

        reader.ReadStartElement("Comment");
        _id = int.Parse(reader.ReadElementString("Id"));
        _username = reader.ReadElementString("Username");
        _content = reader.ReadElementString("Content");
        _dateOfCreation = DateTime.ParseExact(reader.ReadElementString("DateOfCreation"), "dd-MM-yyyy HH:mm", null);
        _dateOfUpdate = DateTime.ParseExact(reader.ReadElementString("DateOfUpdate"), "dd-MM-yyyy HH:mm", null);
        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("Id", Id.ToString());
        writer.WriteElementString("Username", Username);
        writer.WriteElementString("Content", Content);
        writer.WriteElementString("DateOfCreation", DateOfCreation.ToString("dd-MM-yyyy HH:mm"));
        writer.WriteElementString("DateOfUpdate", DateOfUpdate.ToString("dd-MM-yyyy HH:mm"));
    }
}
