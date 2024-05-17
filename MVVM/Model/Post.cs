using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[Serializable]
public class Post : IXmlSerializable
{
    private int _id;
    private int _numberOfLikes;
    private DateTime _creationDate;
    private string _imagePath;
    private string _caption;
    private List<int> _commentIds;

    public int Id => _id;
    public int NumberOfLikes => _numberOfLikes;
    public DateTime CreationDate => _creationDate;
    public string ImagePath => _imagePath;
    public string Caption => _caption;
    public List<int> CommentIds => _commentIds;

    public Post() { }
    public Post(int id, DateTime creationDate, string imagePath, string caption)
    {
        _id = id;
        _numberOfLikes = 0;
        _creationDate = creationDate;
        _imagePath = imagePath;
        _caption = caption;
        _commentIds = new List<int>();
    }
    public void SetNumberOfLikes(int likes) => _numberOfLikes = likes;
    public void SetCreationDate(DateTime creationDate) => _creationDate = creationDate;
    public void SetImagePath(string imagePath) => _imagePath = imagePath;
    public void SetCaption(string caption) => _caption = caption;
    public void SetComments(List<int> comments) => _commentIds = comments;
    public void AddLike() { _numberOfLikes++; }
    public void AddComment(int commentId) { _commentIds.Add(commentId); }

    public XmlSchema? GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement("Post"); // Move to the <Post> element

        // Read private fields from XML
        _id = int.Parse(reader.ReadElementString("_id"));
        _numberOfLikes = int.Parse(reader.ReadElementString("_numberOfLikes"));
        _creationDate = DateTime.ParseExact(reader.ReadElementString("_creationDate"), "dd-MM-yyyy HH:mm", null);
        if(reader.IsStartElement("_imagePath"))
        {
            _imagePath = reader.ReadElementString("_imagePath");
        }
        _caption = reader.ReadElementString("_caption");

        _commentIds = new List<int>();
        // Read _commentIds if it exists
        if (reader.IsStartElement("_commentIds"))
        {
            reader.ReadStartElement("_commentIds");
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "commentId")
                {
                    _commentIds.Add(int.Parse(reader.ReadElementString("commentId")));
                }
                else
                {
                    reader.Read();
                }
            }
        }

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("_id", _id.ToString());
        writer.WriteElementString("_numberOfLikes", _numberOfLikes.ToString());
        writer.WriteElementString("_creationDate", _creationDate.ToString("dd-MM-yyyy HH:mm"));
        writer.WriteElementString("_imagePath", _imagePath);
        writer.WriteElementString("_caption", _caption);

        // Write _commentIds if it exists
        writer.WriteStartElement("_commentIds");
        if (_commentIds is not null)
        {
            foreach (int commentId in _commentIds)
            {
                writer.WriteElementString("commentId", commentId.ToString());
            }
        }
        writer.WriteEndElement(); // End _commentIds element

    }
}