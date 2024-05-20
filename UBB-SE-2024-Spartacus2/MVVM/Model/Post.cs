using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[Serializable]
public class Post : IXmlSerializable
{
    private int id;
    private int numberOfLikes;
    private DateTime creationDate;
    private string imagePath;
    private string caption;
    private List<int> commentIds;

    public int Id => id;
    public int NumberOfLikes => numberOfLikes;
    public DateTime CreationDate => creationDate;
    public string ImagePath => imagePath;
    public string Caption => caption;
    public List<int> CommentIds => commentIds;

    public Post()
    {
        commentIds = new List<int>();
    }

    public Post(int id, DateTime creationDate, string imagePath, string caption)
    {
        this.id = id;
        numberOfLikes = 0;
        this.creationDate = creationDate;
        this.imagePath = imagePath;
        this.caption = caption;
        commentIds = new List<int>();
    }

    public void SetNumberOfLikes(int likes) => numberOfLikes = likes;
    public void SetCreationDate(DateTime creationDate) => this.creationDate = creationDate;
    public void SetImagePath(string imagePath) => this.imagePath = imagePath;
    public void SetCaption(string caption) => this.caption = caption;
    public void SetComments(List<int> comments) => commentIds = comments;
    public void AddLike() => numberOfLikes++;
    public void AddComment(int commentId) => commentIds.Add(commentId);

    public XmlSchema GetSchema() => null;

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement("Post");

        id = int.Parse(reader.ReadElementString(nameof(Id)));
        numberOfLikes = int.Parse(reader.ReadElementString(nameof(NumberOfLikes)));
        creationDate = DateTime.ParseExact(reader.ReadElementString(nameof(CreationDate)), "dd-MM-yyyy HH:mm", null);

        if (reader.IsStartElement(nameof(ImagePath)))
        {
            imagePath = reader.ReadElementString(nameof(ImagePath));
        }

        caption = reader.ReadElementString(nameof(Caption));

        commentIds = new List<int>();
        if (reader.IsStartElement(nameof(CommentIds)))
        {
            reader.ReadStartElement(nameof(CommentIds));
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "CommentId")
                {
                    commentIds.Add(int.Parse(reader.ReadElementString("CommentId")));
                }
                else
                {
                    reader.Read();
                }
            }
            reader.ReadEndElement();
        }

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString(nameof(Id), id.ToString());
        writer.WriteElementString(nameof(NumberOfLikes), numberOfLikes.ToString());
        writer.WriteElementString(nameof(CreationDate), creationDate.ToString("dd-MM-yyyy HH:mm"));
        writer.WriteElementString(nameof(ImagePath), imagePath);
        writer.WriteElementString(nameof(Caption), caption);

        writer.WriteStartElement(nameof(CommentIds));
        if (commentIds != null)
        {
            foreach (int commentId in commentIds)
            {
                writer.WriteElementString("CommentId", commentId.ToString());
            }
        }
        writer.WriteEndElement();
    }
}
