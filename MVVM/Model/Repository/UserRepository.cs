using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Bussiness_social_media.MVVM.Model.Repository
{
    public interface IUserRepository
    {
        List<Account> GetAllAccounts();
        public List<Account> GetAllUsers();
        Account GetAccountByUsername(string username);
        void AddAccount(Account account);
        void DeleteAccount(string username);
        bool UsernameExists(string username);
        bool IsCredentialsValid(string username, string password);
        void ForceAccountSavingToXml();
        public string GetMd5Hash(string input);
    }

    public class UserRepository : IUserRepository
    {
        private List<Account> accounts;
        private string xmlFilePath;

        public UserRepository(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            accounts = new List<Account>();
            LoadAccountsFromXml();
        }

        ~UserRepository()
        {
            SaveAccountsToXml();
        }

        private void LoadAccountsFromXml()
        {
            try
            {
                accounts = new List<Account>();
                if (File.Exists(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Account>), new XmlRootAttribute("ArrayOfAccounts"));

                    using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
                    using (XmlReader reader = XmlReader.Create(fileStream))
                    {
                        accounts = (List<Account>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading accounts from XML: {ex.Message}");
            }
        }

        public List<Account> GetAllUsers()
        {
            return accounts;
        }

        private void SaveAccountsToXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Account>), new XmlRootAttribute("ArrayOfAccounts"));

                using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, accounts);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving accounts to XML: {ex.Message}");
            }
        }

        public List<Account> GetAllAccounts()
        {
            return accounts;
        }

        public Account GetAccountByUsername(string username)
        {
            return accounts.FirstOrDefault(a => a.Username == username);
        }

        public void AddAccount(Account account)
        {
            accounts.Add(account);
            SaveAccountsToXml();
        }

        public void DeleteAccount(string username)
        {
            var accountToRemove = accounts.FirstOrDefault(a => a.Username == username);
            if (accountToRemove != null)
            {
                accounts.Remove(accountToRemove);
                SaveAccountsToXml();
            }
        }

        public bool UsernameExists(string username)
        {
            return accounts.Any(a => a.Username == username);
        }

        public bool IsCredentialsValid(string username, string password)
        {
            return accounts.Any(a => a.Username == username && a.Password == password);
        }

        public void ForceAccountSavingToXml()
        {
            SaveAccountsToXml();
        }

        public string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                if (input is null)
                {
                    return string.Empty;
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
