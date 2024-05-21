using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class Business : IXmlSerializable
{
    private int id;
    private string name;
    private string description;
    private string category;
    private string logo;
    private string banner;
    private string logoFileName;
    private string bannerShort;
    private string phoneNumber;
    private string email;
    private string website;
    private string address;
    private DateTime createdAt;
    private List<string> managerUsernames = new List<string>();
    private List<int> postIds = new List<int>();
    private List<int> reviewIds = new List<int>();
    private List<int> faqIds = new List<int>();

    public int Id => id;
    public string Name => name;
    public string Description => description;
    public string Category => category;
    public string Logo => logo;
    public string Banner => banner;
    public string PhoneNumber => phoneNumber;
    public string Email => email;
    public string Website => website;
    public string Address => address;
    public DateTime CreatedAt => createdAt;
    public List<string> ManagerUsernames => managerUsernames;
    public List<int> PostIds => postIds;
    public List<int> ReviewIds => reviewIds;
    public List<int> FaqIds => faqIds;

    public Business()
    {
    }

    public Business(int id, string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.category = category;
        this.logo = logo;
        this.banner = banner;
        this.phoneNumber = phoneNumber;
        this.email = email;
        this.website = website;
        this.address = address;
        this.createdAt = createdAt;
        this.managerUsernames = managerUsernames;
        this.postIds = postIds;
        this.reviewIds = reviewIds;
        this.faqIds = faqIds;
    }

    public Business(int id, string name, string description, string category, string logoFileName, string logo, string bannerShort, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.category = category;
        this.logoFileName = logoFileName;
        this.logo = logo;
        this.bannerShort = bannerShort;
        this.banner = banner;
        this.phoneNumber = phoneNumber;
        this.email = email;
        this.website = website;
        this.address = address;
        this.createdAt = createdAt;
        this.managerUsernames = managerUsernames;
        this.postIds = postIds;
        this.reviewIds = reviewIds;
        this.faqIds = faqIds;
    }

    public void SetName(string name) => this.name = name;
    public void SetDescription(string description) => this.description = description;
    public void SetCategory(string category) => this.category = category;
    public void SetLogo(string logo) => this.logo = logo;
    public void SetBanner(string banner) => this.banner = banner;
    public void SetPhoneNumber(string phoneNumber) => this.phoneNumber = phoneNumber;
    public void SetEmail(string email) => this.email = email;
    public void SetWebsite(string website) => this.website = website;
    public void SetAddress(string address) => this.address = address;
    public void SetCreatedAt(DateTime createdAt) => this.createdAt = createdAt;
    public void SetLogoFileName(string logoFileName) => this.logoFileName = logoFileName;
    public void SetBannerShort(string bannerShort) => this.bannerShort = bannerShort;
    public void SetManagerUsernames(List<string> usernames) => managerUsernames = usernames;
    public void SetPostIds(List<int> postIds) => this.postIds = postIds;
    public void SetReviewIds(List<int> reviewIds) => this.reviewIds = reviewIds;

    public void SetFaqIds(List<int> faqIds) => this.faqIds = faqIds;

    public void AddManager(string managerUsername) => managerUsernames.Add(managerUsername);

    public override string ToString() => $"Business [ID: {id}, Name: {name}, Category: {category}, Created: {createdAt.ToShortDateString()}]";

    public XmlSchema GetSchema() => null;

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement("Business");

        id = int.Parse(reader.ReadElementString("Id"));
        name = reader.ReadElementString("Name");
        description = reader.ReadElementString("Description");
        category = reader.ReadElementString("Category");

        string binDirectory = "\\bin";
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string pathUntilBin = basePath.Substring(0, basePath.IndexOf(binDirectory));

        logoFileName = reader.ReadElementString("Logo");
        bannerShort = reader.ReadElementString("Banner");

        logo = Path.Combine(pathUntilBin, logoFileName);
        banner = Path.Combine(pathUntilBin, bannerShort);

        phoneNumber = reader.ReadElementString("PhoneNumber");
        email = reader.ReadElementString("Email");
        website = reader.ReadElementString("Website");
        address = reader.ReadElementString("Address");
        createdAt = DateTime.Parse(reader.ReadElementString("CreatedAt"));

        reader.ReadStartElement("ManagerUsernames");
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Username")
            {
                managerUsernames.Add(reader.ReadElementString("Username"));
            }
            else
            {
                reader.Read();
            }
        }
        reader.ReadEndElement();

        if (reader.IsStartElement("PostIds"))
        {
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("PostIds");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "PostId")
                    {
                        postIds.Add(int.Parse(reader.ReadElementString("PostId")));
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

        if (reader.IsStartElement("ReviewIds"))
        {
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("ReviewIds");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ReviewId")
                    {
                        reviewIds.Add(int.Parse(reader.ReadElementString("ReviewId")));
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

        if (reader.IsStartElement("FaqIds"))
        {
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("FaqIds");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "FaqId")
                    {
                        faqIds.Add(int.Parse(reader.ReadElementString("FaqId")));
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
        writer.WriteElementString("Id", id.ToString());
        writer.WriteElementString("Name", name);
        writer.WriteElementString("Description", description);
        writer.WriteElementString("Category", category);
        writer.WriteElementString("Logo", logoFileName);
        writer.WriteElementString("Banner", bannerShort);
        writer.WriteElementString("PhoneNumber", phoneNumber);
        writer.WriteElementString("Email", email);
        writer.WriteElementString("Website", website);
        writer.WriteElementString("Address", address);
        writer.WriteElementString("CreatedAt", createdAt.ToString("dd-MM-yyyy HH:mm"));

        writer.WriteStartElement("ManagerUsernames");
        foreach (string username in managerUsernames)
        {
            writer.WriteElementString("Username", username);
        }
        writer.WriteEndElement();

        writer.WriteStartElement("PostIds");
        foreach (int postId in postIds)
        {
            writer.WriteElementString("PostId", postId.ToString());
        }
        writer.WriteEndElement();

        writer.WriteStartElement("ReviewIds");
        foreach (int reviewId in reviewIds)
        {
            writer.WriteElementString("ReviewId", reviewId.ToString());
        }
        writer.WriteEndElement();

        writer.WriteStartElement("FaqIds");
        foreach (int faqId in faqIds)
        {
            writer.WriteElementString("FaqId", faqId.ToString());
        }
        writer.WriteEndElement();
    }
}