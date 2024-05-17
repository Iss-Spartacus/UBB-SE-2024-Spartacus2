using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace bussiness_social_media.MVVM.Model.Repository
{
    public interface IUserRepository
    {
        public void AddAccount(Account user);
        public void DeleteUser(string username);

        public bool UsernameExists(string username);

        public bool IsCredentialsValid(string username, string password);
        public List<Account> GetAllUsers();

        // Method to compute MD5 hash
        public string GetMd5Hash(string input);
    }
    public class UserRepository : IUserRepository
    {
        private string _xmlFilePath;
        private List<Account> _users;

        public UserRepository(string xmlFilePATH)
        {
            _users = new List<Account>();
            _xmlFilePath = xmlFilePATH;
            LoadUsersFromXml();
        }

        ~UserRepository() {
            SaveUsersToXml();
        }

        // Some hard coded users
        public void populateRepository()
        {
            Account Account1 = new Account
            (
                "admin",
                GetMd5Hash("password123")
            );
            _users.Add(Account1);

            Account Account2 = new Account
            (
                "alice_smith",
                GetMd5Hash("qwerty456")
            );
            _users.Add(Account2);

            Account Account3 = new Account
            (
                "bob_jackson",
                GetMd5Hash("abc123")
            );
            _users.Add(Account3);

            Account account4 = new Account
            (
                "ana_maria",
                GetMd5Hash("8a8n8a9"));
            _users.Add(account4);

            Account account5 = new Account
            (
                "kevin_smith",
                GetMd5Hash("k3vin"));
            _users.Add(account5);
        }

        private void LoadUsersFromXml()
        {
            try
            {
                if (File.Exists(_xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Account), new XmlRootAttribute("Account"));

                    _users = new List<Account>();

                    using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Open))
                    {
                        using (XmlReader reader = XmlReader.Create(fileStream))
                        {
                            // Move to the first Business element
                            while (reader.ReadToFollowing("Account"))
                            {
                                // Deserialize each Business element and add it to the list
                                Account user = (Account)serializer.Deserialize(reader);
                                _users.Add(user);
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where the XML file doesn't exist
                    _users = new List<Account>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something terrible, terrible has happened during the execution of the program. Show this to your local IT guy. UserRepository.LoadUsersFromXml():" + ex.Message);
            }
        }

        private void SaveUsersToXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Account>), new XmlRootAttribute("ArrayOfAccounts"));

            using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, _users);
            }
        }

        public void AddAccount(Account user)
        {
            _users.Add(user);
            SaveUsersToXml();
        }

        public void DeleteUser(string username)
        {
            Account userToRemove = _users.Find(u => u.Username == username);
            if (userToRemove != null)
            {
                _users.Remove(userToRemove);
                SaveUsersToXml();
            }
        }

        public bool UsernameExists(string username)
        {
            return _users.Exists(u => u.Username == username);
        }

        public bool IsCredentialsValid(string username, string password)
        {
            return _users.Exists(u => u.Username == username && u.Password == password);
        }

        public List<Account> GetAllUsers()
        {
            return _users;
        }

        // Method to compute MD5 hash
        public string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                if (input is null)
                {
                    return "";
                }
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
