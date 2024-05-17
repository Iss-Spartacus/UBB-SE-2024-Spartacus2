using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[Serializable]
public class Account : IXmlSerializable
{
    private string _username;
    private string _password;
    private string _firstname;
    private string _lastname;
    private string _email;
    private string _gender;
    private DateTime _birthday;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
        }
    }

    public string Firstname
    {
        get => _firstname;
        set
        {
            _firstname = value;
        }
    }

    public string Lastname
    {
        get => _lastname;
        set
        {
            _lastname = value;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
        }
    }

    public DateTime Birthday
    {
        get => _birthday;
        set
        {
            _birthday = value;
        }
    }

    public string Gender
    {
        get => _gender;
        private set
        {
            _gender = value;
        }
    }

    public Account(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public Account(string username, string password,string firstname,string lastname,string email,string day,string month,string year,string gender)
    {
        _username = username;
        _password = password;
        _firstname = firstname;
        _lastname = lastname;
        _email = email;
        _birthday = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        _gender = gender;
    }

    public Account()
    {
    }

    public override string ToString()
    {
        return $"Username: {_username}, Password: {_password}";
    }

    public XmlSchema? GetSchema()
    {
        return null; // Not needed
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement("Account"); // Move to the <Account> element

        _username = reader.ReadElementString("_username");
        _password = reader.ReadElementString("_password");

        reader.ReadEndElement(); // Close the <Account> element
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("_username", _username);
        writer.WriteElementString("_password", _password);
    }
}
