using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Review : IXmlSerializable
{
    public int Id { get; private set; }
    public string UserName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public string Title { get; set; }
    public string ImagePath { get; set; }
    public DateTime DateOfCreation { get; private set; }
    public int AdminCommentId { get; set; }

    public Review(int id, string userName, int rating, string comment, string title, string imagePath, DateTime dateOfCreation)
    {
        Id = id;
        UserName = userName;
        Rating = rating;
        Comment = comment;
        Title = title;
        ImagePath = imagePath;
        DateOfCreation = dateOfCreation;
        AdminCommentId = -1;
    }

    public Review()
    {
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
            Id = int.Parse(reader.ReadElementString("Id"));
            UserName = reader.ReadElementString("UserName");
            Rating = int.Parse(reader.ReadElementString("Rating"));
            Comment = reader.ReadElementString("Comment");
            Title = reader.ReadElementString("Title");
            ImagePath = reader.ReadElementString("ImagePath");
            DateOfCreation = DateTime.Parse(reader.ReadElementString("DateOfCreation"));
            AdminCommentId = int.Parse(reader.ReadElementString("AdminComment"));
            reader.ReadEndElement();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("Id", Id.ToString());
        writer.WriteElementString("UserName", UserName);
        writer.WriteElementString("Rating", Rating.ToString());
        writer.WriteElementString("Comment", Comment);
        writer.WriteElementString("Title", Title);
        writer.WriteElementString("ImagePath", ImagePath);
        writer.WriteElementString("DateOfCreation", DateOfCreation.ToString());
        writer.WriteElementString("AdminComment", AdminCommentId.ToString());
    }
}
