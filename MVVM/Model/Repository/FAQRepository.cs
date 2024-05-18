using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Bussiness_social_media.MVVM.Model.Repository
{
    public interface IFAQRepository
    {
        List<FAQ> GetAllFAQs();
        FAQ GetFAQById(int id);
        int AddFAQ(string faqQuestion, string faqAnswer);
        void UpdateFAQ(int faqID, string newFaqQuestion, string newFaqAnswer);
        void DeleteFAQ(int id);
    }

    public class FAQRepository : IFAQRepository
    {
        private List<FAQ> faqs;
        private string xmlFilePath;

        public FAQRepository(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            faqs = new List<FAQ>();
            LoadFAQsFromXml();
        }

        ~FAQRepository()
        {
            SaveFAQsToXml();
        }

        private void LoadFAQsFromXml()
        {
            try
            {
                faqs = new List<FAQ>();
                if (File.Exists(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<FAQ>), new XmlRootAttribute("ArrayOfFAQ"));

                    using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
                    using (XmlReader reader = XmlReader.Create(fileStream))
                    {
                        faqs = (List<FAQ>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading FAQs from XML: {ex.Message}");
            }
        }

        private void SaveFAQsToXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<FAQ>), new XmlRootAttribute("ArrayOfFAQ"));

                using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, faqs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving FAQs to XML: {ex.Message}");
            }
        }

        public List<FAQ> GetAllFAQs()
        {
            return faqs;
        }

        public FAQ GetFAQById(int id)
        {
            return faqs.FirstOrDefault(f => f.Id == id);
        }

        public int AddFAQ(string faqQuestion, string faqAnswer)
        {
            int newID = GetNextId();
            FAQ faq = new FAQ(newID, faqQuestion, faqAnswer);
            faqs.Add(faq);
            SaveFAQsToXml();
            return newID;
        }

        public void UpdateFAQ(int faqID, string newFaqQuestion, string newFaqAnswer)
        {
            FAQ existingFAQ = faqs.FirstOrDefault(f => f.Id == faqID);
            if (existingFAQ != null)
            {
                existingFAQ.Question = newFaqQuestion;
                existingFAQ.Answer = newFaqAnswer;
                SaveFAQsToXml();
            }
        }

        public void DeleteFAQ(int id)
        {
            var faqToRemove = faqs.FirstOrDefault(f => f.Id == id);
            if (faqToRemove != null)
            {
                faqs.Remove(faqToRemove);
                SaveFAQsToXml();
            }
        }

        private int GetNextId()
        {
            return faqs.Count > 0 ? faqs.Max(f => f.Id) + 1 : 1;
        }
    }
}
