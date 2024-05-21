using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Bussiness_social_media.MVVM.Model.Repository
{
    public interface IFileSystem
    {
        bool FileExists(string path);
        Stream OpenFile(string path, FileMode mode);
        void CreateFile(string path);
    }

    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public Stream OpenFile(string path, FileMode mode)
        {
            return File.Open(path, mode);
        }

        public void CreateFile(string path)
        {
            File.Create(path);
        }
    }



    public interface IBusinessRepository
    {
        List<Business> GetAllBusinesses();
        Business GetBusinessById(int id);
        void AddBusiness(string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds);
        void AddBusiness(string name, string description, string category, string logoShort, string logo, string bannerShort, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds);
        void UpdateBusiness(Business business);
        void DeleteBusiness(int id);
        List<Business> SearchBusinesses(string keyword);
        void SaveBusinessesToXml();
    }

    public class BusinessRepository : IBusinessRepository
    {
        private List<Business> businesses;
        private string xmlFilePath;
        private IFileSystem fileSystem;

        private static Random random = new Random();

        public BusinessRepository()
        {
            businesses = new List<Business>();
            Generate10RandomBusineses();
        }

      
            

        public BusinessRepository(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            this.businesses = new List<Business>();
        }

        public BusinessRepository(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            LoadBusinessesFromXml();
        }

        ~BusinessRepository()
        {
            SaveBusinessesToXml();
        }

        private void Generate10RandomBusineses()
        {
            string[] categories = { "Food", "Tech", "Retail", "Finance", "Services" };
            string[] namePrefixes = { "The", "Super", "Awesome", "Modern", "Innovative" };
            string[] nameSuffixes = { "Co.", "Inc.", "Hub", "Solutions", "Place" };

            for (int i = 0; i < 10; i++)
            {
                string name = $"{namePrefixes[random.Next(namePrefixes.Length)]} {nameSuffixes[random.Next(nameSuffixes.Length)]}";
                string description = "A cool business doing cool things!";
                string category = categories[random.Next(categories.Length)];

                string binDirectory = "\\bin";
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string pathUntilBin;

                int index = basePath.IndexOf(binDirectory);
                pathUntilBin = basePath.Substring(0, index);

                // Placeholder values for logo, banner, etc.
                string logoShort = $"Assets\\Images\\scat{i + 1}.jpg";
                string logo = Path.Combine(pathUntilBin, logoShort);
                string bannerShort = $"Assets\\Images\\banner{i + 1}.jpg";
                string banner = Path.Combine(pathUntilBin, bannerShort);
                string phoneNumber = GenerateRandomPhoneNumber();
                string email = $"business{i}@example.com";
                string website = $"http://{name.Replace(' ', '-')}.com";
                string address = "123 Main St., Anytown, CA";
                List<string> managerUsernames = new List<string> { "admin" };
                List<int> postIds = new List<int>();
                List<int> reviewIds = new List<int>();
                List<int> faqIds = new List<int>();

                AddBusiness(name, description, category, logoShort, logo, bannerShort, banner, phoneNumber, email, website, address, DateTime.Now, managerUsernames, postIds, reviewIds, faqIds);
            }
        }

        private void LoadBusinessesFromXml()
        {
            try
            {
                if (File.Exists(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Business), new XmlRootAttribute("Business"));
                    businesses = new List<Business>();

                    using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
                    {
                        using (XmlReader reader = XmlReader.Create(fileStream))
                        {
                            // Move to the first Business element
                            while (reader.ReadToFollowing("Business"))
                            {
                                // Deserialize each Business element and add it to the list
                                Business business = (Business)serializer.Deserialize(reader);
                                businesses.Add(business);
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where the XML file doesn't exist
                    businesses = new List<Business>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something terrible, terrible has happened during the execution of the program. Show this to your local IT guy: " + ex.Message);
            }
        }

        public void SaveBusinessesToXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Business>), new XmlRootAttribute("ArrayOfBusiness"));

            using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, businesses);
            }
        }

        private string GenerateRandomPhoneNumber()
        {
            return $"{random.Next(100, 999)}-{random.Next(100, 999)}-{random.Next(1000, 9999)}";
        }

        public List<Business> GetAllBusinesses() => businesses;

        public Business GetBusinessById(int id) => businesses.FirstOrDefault(b => b.Id == id);

        public void AddBusiness(string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
        {
            Business business = new Business(GetNextId(), name, description, category, logo, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);
            businesses.Add(business);
            SaveBusinessesToXml();
        }

        public void AddBusiness(string name, string description, string category, string logoShort, string logo, string bannerShort, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
        {
            Business business = new Business(GetNextId(), name, description, category, logoShort, logo, bannerShort, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);
            businesses.Add(business);
            SaveBusinessesToXml();
        }

        public void UpdateBusiness(Business business)
        {
            var existingBusiness = businesses.FirstOrDefault(b => b.Id == business.Id);
            if (existingBusiness != null)
            {
                existingBusiness.SetName(business.Name);
                existingBusiness.SetDescription(business.Description);
                existingBusiness.SetCategory(business.Category);
                existingBusiness.SetLogo(business.Logo);
                existingBusiness.SetBanner(business.Banner);
                existingBusiness.SetPhoneNumber(business.PhoneNumber);
                existingBusiness.SetEmail(business.Email);
                existingBusiness.SetWebsite(business.Website);
                existingBusiness.SetAddress(business.Address);
                existingBusiness.SetManagerUsernames(business.ManagerUsernames);
                existingBusiness.SetPostIds(business.PostIds);
                existingBusiness.SetReviewIds(business.ReviewIds);
                existingBusiness.SetFaqIds(business.FaqIds);
                SaveBusinessesToXml();
            }
        }

        public void DeleteBusiness(int id)
        {
            var businessToRemove = businesses.FirstOrDefault(b => b.Id == id);
            if (businessToRemove != null)
            {
                businesses.Remove(businessToRemove);
                SaveBusinessesToXml();
            }
        }

        private int GetNextId()
        {
            return businesses.Count > 0 ? businesses.Max(b => b.Id) + 1 : 1;
        }

        public List<Business> SearchBusinesses(string keyword)
        {
            if (keyword == string.Empty)
            {
                return businesses;
            }
            var filteredBusinesses = businesses.Where(b =>
            (string.IsNullOrEmpty(keyword) ||
            b.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToList();
            return filteredBusinesses;
        }
    }
}
