using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace bussiness_social_media.MVVM.Model.Repository
{
    public interface IReviewRepository
    {
        List<Review> GetAllReviews();
        Review GetReviewById(int id);
        int AddReview(string userName, int rating, string comment, string title, string imagePath, DateTime dateOfCreation);
        void UpdateReview(int id, int newRating, string newComment, string newTitle, string newImagePath);
        void DeleteReview(int id);
        void ForceReviewSavingToXml();
    }

    public class ReviewRepository : IReviewRepository
    {
        private List<Review> _reviews;
        private string _xmlFilePath;

        public ReviewRepository(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _reviews = new List<Review>();
            LoadReviewsFromXml();
        }

        ~ReviewRepository()
        {
            SaveReviewsToXml();
        }

        private void LoadReviewsFromXml()
        {
            try{
                if (File.Exists(_xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Review), new XmlRootAttribute("Review"));

                    _reviews = new List<Review>();

                    using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Open))
                    {
                        using (XmlReader reader = XmlReader.Create(fileStream))
                        {
                            // Move to the first Business element
                            while (reader.ReadToFollowing("Review"))
                            {
                                // Deserialize each Business element and add it to the list
                                Review review = (Review)serializer.Deserialize(reader);
                                _reviews.Add(review);
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where the XML file doesn't exist
                    _reviews = new List<Review>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something terrible, terrible has happened during the execution of the program. Show this to your local IT guy. ReviewRepository.LoadReviewsFromXml():" + ex.Message);
            }
        }

        private void SaveReviewsToXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Review>), new XmlRootAttribute("ArrayOfReview"));

            using (FileStream fileStream = new FileStream(_xmlFilePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, _reviews);
            }
        }

        public List<Review> GetAllReviews()
        {
            return _reviews;
        }

        public Review GetReviewById(int id)
        {
            return _reviews.FirstOrDefault(r => r.GetReviewId() == id);
        }

        public int AddReview(string userName, int rating, string comment, string title, string imagePath, DateTime dateOfCreation)
        {
            int newId = GetNextId();
            Review review = new Review(newId, userName, rating, comment, title, imagePath, dateOfCreation);
            _reviews.Add(review);
            SaveReviewsToXml();
            return newId;
        }

        public void UpdateReview(int id, int newRating, string newComment, string newTitle, string newImagePath)
        {
            var existingReview = GetReviewById(id);
            if (existingReview != null)
            {
                existingReview.SetUserName(newComment);
                existingReview.SetRating(newRating);
                existingReview.SetComment(newComment);
                existingReview.SetTitle(newTitle);
                existingReview.SetImagePath(newImagePath);
                SaveReviewsToXml();
            }
        }

        public void DeleteReview(int id)
        {
            var reviewToRemove = _reviews.FirstOrDefault(r => r.GetReviewId() == id);
            if (reviewToRemove != null)
            {
                _reviews.Remove(reviewToRemove);
                SaveReviewsToXml();
            }
        }
        private int GetNextId()
        {
            return _reviews.Count > 0 ? _reviews.Max(r => r.GetReviewId()) + 1 : 1;
        }

        public void ForceReviewSavingToXml()
        {
            SaveReviewsToXml();
        }
    }
}
