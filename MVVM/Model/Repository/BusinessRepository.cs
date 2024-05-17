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

namespace bussiness_social_media.MVVM.Model.Repository
{
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
        private List<Business> _businesses;
        private string _xmlFilePath;

        private static Random _random = new Random();
        public BusinessRepository()
        {
            _businesses = new List<Business>();
            generate10RandomBusineses();
        }

        public BusinessRepository(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _businesses = new List<Business>();
            
            LoadBusinessesFromXml();
        }

        ~BusinessRepository()
        {
            SaveBusinessesToXml();
        }

        private void generate10RandomBusineses()
        {
            string[] categories = { "Food", "Tech", "Retail", "Finance", "Services" };
            string[] namePrefixes = { "The", "Super", "Awesome", "Modern", "Innovative" };
            string[] nameSuffixes = { "Co.", "Inc.", "Hub", "Solutions", "Place" };

            for (int i = 0; i < 10; i++)
            {
                string name = namePrefixes[_random.Next(namePrefixes.Length)] + " " + nameSuffixes[_random.Next(nameSuffixes.Length)];
                string description = "A cool business doing cool things!";
                string category = categories[_random.Next(categories.Length)];

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
                if (File.Exists(_xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Business), new XmlRootAttribute("Business"));

                    _businesses = new List<Business>();

                    using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Open))
                    {
                        using (XmlReader reader = XmlReader.Create(fileStream))
                        {
                            // Move to the first Business element
                            while (reader.ReadToFollowing("Business"))
                            {
                                // Deserialize each Business element and add it to the list
                                Business business = (Business)serializer.Deserialize(reader);
                                _businesses.Add(business);
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where the XML file doesn't exist
                    _businesses = new List<Business>();
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

            using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, _businesses);
            }
        }

        private string GenerateRandomPhoneNumber()
        {
            return $"{_random.Next(100, 999)}-{_random.Next(100, 999)}-{_random.Next(1000, 9999)}";
        }
        public List<Business> GetAllBusinesses()
        {
            return _businesses;
        }

        public Business GetBusinessById(int id)
        {
            return _businesses.FirstOrDefault(b => b.Id == id);
        }

        public void AddBusiness(string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
        {
            Business business = new Business(_getNextId(), name, description, category, logo, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);
            _businesses.Add(business);
            SaveBusinessesToXml();
        }

        public void AddBusiness(string name, string description, string category, string logoShort, string logo, string bannerShort, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
        {
            Business business = new Business(_getNextId(), name, description, category, logoShort, logo, bannerShort, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);
            _businesses.Add(business);
            SaveBusinessesToXml();
        }

        public void UpdateBusiness(Business business)
        {
            var existingBusiness = _businesses.FirstOrDefault(b => b.Id == business.Id);
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
            var businessToRemove = _businesses.FirstOrDefault(b => b.Id == id);
            if (businessToRemove != null)
            {
                _businesses.Remove(businessToRemove);
                SaveBusinessesToXml();
            }
        }

        private int _getNextId()
        {
            return _businesses.Count > 0 ? _businesses.Max(b => b.Id) + 1 : 1;
        }


        public List<Business> SearchBusinesses(string keyword)
        {
            if(keyword == "")
                return _businesses;
            var filteredBusinesses = _businesses.Where(b =>
            (string.IsNullOrEmpty(keyword) ||
            b.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            ).ToList();
            return filteredBusinesses;

        }
    }
}
