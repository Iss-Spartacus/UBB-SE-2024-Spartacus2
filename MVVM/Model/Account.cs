using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[Serializable]
public class Account : IXmlSerializable
{
    private string username;
    private string password;
    private string firstname;
    private string lastname;
    private string email;
    private string gender;
    private DateTime birthday;

    public string Username
    {
        get => username;
        set => username = value;
    }

    public string Password
    {
        get => password;
        set => password = value;
    }

    public string Firstname
    {
        get => firstname;
        set => firstname = value;
    }

    public string Lastname
    {
        get => lastname;
        set => lastname = value;
    }

    public string Email
    {
        get => email;
        set => email = value;
    }

    public DateTime Birthday
    {
        get => birthday;
        set => birthday = value;
    }

    public string Gender
    {
        get => gender;
        private set => gender = value;
    }

    public Account(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public Account(string username, string password, string firstname, string lastname, string email, string day, string month, string year, string gender)
    {
        this.username = username;
        this.password = password;
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.birthday = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        this.gender = gender;
    }

    public Account()
    {
    }

    public override string ToString()
    {
        return $"Username: {username}, Password: {password}";
    }

    public XmlSchema GetSchema()
    {
        return null; // Not needed
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement("Account"); // Move to the <Account> element

        username = reader.ReadElementString(nameof(username));
        password = reader.ReadElementString(nameof(password));
        firstname = reader.ReadElementString(nameof(firstname));
        lastname = reader.ReadElementString(nameof(lastname));
        email = reader.ReadElementString(nameof(email));
        gender = reader.ReadElementString(nameof(gender));
        birthday = DateTime.Parse(reader.ReadElementString(nameof(birthday)));

        reader.ReadEndElement(); // Close the <Account> element
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString(nameof(username), username);
        writer.WriteElementString(nameof(password), password);
        writer.WriteElementString(nameof(firstname), firstname);
        writer.WriteElementString(nameof(lastname), lastname);
        writer.WriteElementString(nameof(email), email);
        writer.WriteElementString(nameof(gender), gender);
        writer.WriteElementString(nameof(birthday), birthday.ToString("yyyy-MM-dd"));
    }
}
