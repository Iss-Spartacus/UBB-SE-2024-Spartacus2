using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Review : IXmlSerializable
{
    private int _id;
    private string _userName;
    private int _rating;
    private string _comment;
    private string _title;
    private string _imagePath;
    private DateTime _dateOfCreation;
    private int _adminCommentId;

    public string UserName => _userName;
    public int Rating => _rating;
    public string Comment => _comment;
    public string Title => _title;

    public Review(int id, string userName, int rating, string comment, string title, string imagePath, DateTime dateOfCreation)
    {
        _id = id;
        _userName = userName;
        _rating = rating;
        _comment = comment;
        _title = title;
        _imagePath = imagePath;
        _dateOfCreation = dateOfCreation;
        _adminCommentId = -1;
    }

    public int AdminCommentId { get => _adminCommentId; set => _adminCommentId = value;  }

    public Review() { }
    public int GetRating() { return _rating; }
    public int GetReviewId() { return _id; }
    public string GetComment() { return _comment; }
    public string GetTitle() { return _title; }
    public string GetImagePath() { return _imagePath; }
    public string GetDateOfCreation() { return _dateOfCreation.ToString(); }
    public string GetUserName() { return _userName; }

    public void SetUserName(string userName) { _userName = userName; }
    public void SetTitle(string title) { _title = title; }
    public void SetImagePath(string imagePath) { _imagePath = imagePath; }
    public void SetRating(int rating) { _rating = rating; }
    public void SetComment(string comment) { _comment = comment; }

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
            _id = int.Parse(reader.ReadElementString("Id"));
            _userName = reader.ReadElementString("UserName");
            _rating = int.Parse(reader.ReadElementString("Rating"));
            _comment = reader.ReadElementString("Comment");
            _title = reader.ReadElementString("Title");
            _imagePath = reader.ReadElementString("ImagePath");
            _dateOfCreation = DateTime.Parse(reader.ReadElementString("DateOfCreation"));
            _adminCommentId = int.Parse(reader.ReadElementString("AdminComment"));
            reader.ReadEndElement();
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("Id", _id.ToString());
        writer.WriteElementString("UserName", _userName);
        writer.WriteElementString("Rating", _rating.ToString());
        writer.WriteElementString("Comment", _comment);
        writer.WriteElementString("Title", _title);
        writer.WriteElementString("ImagePath", _imagePath);
        writer.WriteElementString("DateOfCreation", _dateOfCreation.ToString());
        writer.WriteElementString("AdminComment", _adminCommentId.ToString());
    }
}
