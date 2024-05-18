using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness_social_media.MVVM.Model.Repository;
namespace Bussiness_social_media.Services
{
    public interface IFAQService
    {
        List<FAQ> GetAllFAQs();
        FAQ GetFAQById(int id);
        int AddFAQ(string faqQuestion, string faqAnswer);
        void UpdateFAQ(int faqID, string newFaqQuestion, string newFaqAnswer);
        void DeleteFAQ(int faqID);
    }
    public class FAQService : IFAQService
    {
        private IFAQRepository faqRepository;

        public FAQService(IFAQRepository faqRepository)
        {
            this.faqRepository = faqRepository;
        }
        public int AddFAQ(string faqQuestion, string faqAnswer)
        {
            return faqRepository.AddFAQ(faqQuestion, faqAnswer);
        }

        public void DeleteFAQ(int faqID)
        {
            faqRepository.DeleteFAQ(faqID);
        }

        public List<FAQ> GetAllFAQs()
        {
            return faqRepository.GetAllFAQs();
        }

        public FAQ GetFAQById(int faqID)
        {
            return faqRepository.GetFAQById(faqID);
        }

        public void UpdateFAQ(int faqID, string newFaqQuestion, string newFaqAnswer)
        {
            faqRepository.UpdateFAQ(faqID, newFaqQuestion, newFaqAnswer);
        }
    }
}
