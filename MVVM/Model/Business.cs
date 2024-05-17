using System;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class Business : IXmlSerializable
{
    private int _id;
    private string _name;
    private string _description;
    private string _category;
    private string _logo;
    private string _banner;
    private string _logoFileName;
    private string _bannerShort;
    private string _phoneNumber;
    private string _email;
    private string _website;
    private string _address;
    private DateTime _createdAt;
    private List<string> _managerUsernames = new List<string>();
    private List<int> _postIds = new List<int>();
    private List<int> _reviewIds = new List<int>();
    private List<int> _faqIds = new List<int>();

    public int Id => _id;
    public string Name => _name;
    public string Description => _description;
    public string Category => _category;
    public string Logo => _logo;
    public string Banner => _banner;
    public string PhoneNumber => _phoneNumber;
    public string Email => _email;
    public string Website => _website;
    public string Address => _address;
    public DateTime CreatedAt => _createdAt;
    public List<string> ManagerUsernames => _managerUsernames;
    public List<int> PostIds => _postIds;
    public List<int> ReviewIds => _reviewIds;

    public List<int> FaqIds => _faqIds;

    public Business()
    {

    }
    public Business(int id, string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
    {
        _id = id;
        _name = name;
        _description = description;
        _category = category;
        _logo = logo;
        _banner = banner;
        _phoneNumber = phoneNumber;
        _email = email;
        _website = website;
        _address = address;
        _createdAt = createdAt;
        _managerUsernames = managerUsernames;
        _postIds = postIds;
        _reviewIds = reviewIds;
        _faqIds = faqIds;
    }

    public Business(int id, string name, string description, string category, string logoFileName, string logo, string bannerShort, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
    {
        _id = id;
        _name = name;
        _description = description;
        _category = category;
        _logoFileName = logoFileName;
        _logo = logo;
        _bannerShort = bannerShort;
        _banner = banner;
        _phoneNumber = phoneNumber;
        _email = email;
        _website = website;
        _address = address;
        _createdAt = createdAt;
        _managerUsernames = managerUsernames;
        _postIds = postIds;
        _reviewIds = reviewIds;
        _faqIds = faqIds;
    }

    public void SetName(string name) => _name = name;
    public void SetDescription(string description) => _description = description;
    public void SetCategory(string category) => _category = category;
    public void SetLogo(string logo) => _logo = logo;
    public void SetBanner(string banner) => _banner = banner;
    public void SetPhoneNumber(string phoneNumber) => _phoneNumber = phoneNumber;
    public void SetEmail(string email) => _email = email;
    public void SetWebsite(string website) => _website = website;
    public void SetAddress(string address) => _address = address;
    public void SetCreatedAt(DateTime createdAt) => _createdAt = createdAt;
    public void SetLogoFileName(string logoFileName) => _logoFileName = logoFileName;
    public void SetBannerShort(string bannerShort) => _bannerShort = bannerShort;
    public void SetManagerUsernames(List<string> usernames) => _managerUsernames = usernames;
    public void SetPostIds(List<int> postIds) => _postIds = postIds;
    public void SetReviewIds(List<int> reviewIds) => _reviewIds = reviewIds;
    public void SetFaqIds(List<int> faqIds) => _faqIds = faqIds;

    public void AddManager(string managerUsername)
    {
        _managerUsernames.Add(managerUsername);
    }

    public override string ToString()
    {
        return $"Business [ID: {_id}, Name: {_name}, Category: {_category}, Created: {_createdAt.ToShortDateString()}]";
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement("Business"); 

        _id = int.Parse(reader.ReadElementString("_id"));
        _name = reader.ReadElementString("_name");
        _description = reader.ReadElementString("_description");
        _category = reader.ReadElementString("_category");

        string binDirectory = "\\bin";
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string pathUntilBin;


        int index = basePath.IndexOf(binDirectory);
        pathUntilBin = basePath.Substring(0, index);

        _logoFileName = reader.ReadElementString("_logo");
        _bannerShort = reader.ReadElementString("_banner");

         _logo = Path.Combine(pathUntilBin, _logoFileName);
        _banner = Path.Combine(pathUntilBin, _bannerShort);

        _phoneNumber = reader.ReadElementString("_phoneNumber");
        _email = reader.ReadElementString("_email");
        _website = reader.ReadElementString("_website");
        _address = reader.ReadElementString("_address");
        _createdAt = DateTime.Parse(reader.ReadElementString("_createdAt"));

        reader.ReadStartElement("_managerUsernames");
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "username")
            {
                _managerUsernames.Add(reader.ReadElementString("username"));
            }
            else
            {
                reader.Read();
            }
        }
        reader.ReadEndElement();

        if (reader.IsStartElement("_postIds"))
        {
            if (!reader.IsEmptyElement) 
            {
                reader.ReadStartElement("_postIds");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "postId")
                    {
                        _postIds.Add(int.Parse(reader.ReadElementString("postId")));
                    }
                    else
                    {
                        reader.Read();
                    }
                }
                reader.ReadEndElement(); 
            }
            else
            {
                reader.Read(); 
            }
        }

        if (reader.IsStartElement("_reviewIds"))
        {
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("_reviewIds");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "reviewId")
                    {
                        _reviewIds.Add(int.Parse(reader.ReadElementString("reviewId")));
                    }
                    else
                    {
                        reader.Read();
                    }
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read(); 
            }
        }

        if (reader.IsStartElement("_faqIds"))
        {
            if (!reader.IsEmptyElement) 
            {
                reader.ReadStartElement("_faqIds");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "faqId")
                    {
                        _faqIds.Add(int.Parse(reader.ReadElementString("faqId")));
                    }
                    else
                    {
                        reader.Read();
                    }
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read(); 
            }
        }


        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("_id", _id.ToString());
        writer.WriteElementString("_name", _name);
        writer.WriteElementString("_description", _description);
        writer.WriteElementString("_category", _category);
        writer.WriteElementString("_logo", _logoFileName);
        writer.WriteElementString("_banner", _bannerShort);
        writer.WriteElementString("_phoneNumber", _phoneNumber);
        writer.WriteElementString("_email", _email);
        writer.WriteElementString("_website", _website);
        writer.WriteElementString("_address", _address);
        writer.WriteElementString("_createdAt", _createdAt.ToString());

        writer.WriteStartElement("_managerUsernames");
        foreach (string username in _managerUsernames)
        {
            writer.WriteElementString("username", username);
        }
        writer.WriteEndElement(); 

        writer.WriteStartElement("_postIds");
        foreach (int postId in _postIds)
        {
            writer.WriteElementString("postId", postId.ToString());
        }
        writer.WriteEndElement(); 

        writer.WriteStartElement("_reviewIds");
        foreach (int reviewId in _reviewIds)
        {
            writer.WriteElementString("reviewId", reviewId.ToString());
        }
        writer.WriteEndElement();

        writer.WriteStartElement("_faqIds");
        foreach (int faqId in _faqIds)
        {
            writer.WriteElementString("faqId", faqId.ToString());
        }
        writer.WriteEndElement();

    }

}
