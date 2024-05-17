using bussiness_social_media.MVVM.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussiness_social_media.Services
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
        private IFAQRepository _faqRepository;

        public FAQService(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }
        public int AddFAQ(string faqQuestion, string faqAnswer)
        {
            return _faqRepository.AddFAQ(faqQuestion, faqAnswer);
        }

        public void DeleteFAQ(int faqID)
        {
            _faqRepository.DeleteFAQ(faqID);
        }

        public List<FAQ> GetAllFAQs()
        {
            return _faqRepository.GetAllFAQs();
        }

        public FAQ GetFAQById(int faqID)
        {
            return _faqRepository.GetFAQById(faqID);
        }

        public void UpdateFAQ(int faqID, string newFaqQuestion, string newFaqAnswer)
        {
            _faqRepository.UpdateFAQ(faqID, newFaqQuestion, newFaqAnswer);
        }
    }
}
