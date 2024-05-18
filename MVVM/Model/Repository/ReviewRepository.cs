using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Bussiness_social_media.MVVM.Model.Repository
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
        private List<Review> reviews;
        private string xmlFilePath;

        public ReviewRepository(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            reviews = new List<Review>();
            LoadReviewsFromXml();
        }

        ~ReviewRepository()
        {
            SaveReviewsToXml();
        }

        private void LoadReviewsFromXml()
        {
            try
            {
                reviews = new List<Review>();
                if (File.Exists(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Review>), new XmlRootAttribute("ArrayOfReview"));

                    using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
                    using (XmlReader reader = XmlReader.Create(fileStream))
                    {
                        reviews = (List<Review>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading reviews from XML: {ex.Message}");
            }
        }

        private void SaveReviewsToXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Review>), new XmlRootAttribute("ArrayOfReview"));

                using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Create))
                {
                    serializer.Serialize(fileStream, reviews);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving reviews to XML: {ex.Message}");
            }
        }

        public List<Review> GetAllReviews()
        {
            return reviews;
        }

        public Review GetReviewById(int id)
        {
            return reviews.FirstOrDefault(r => r.Id == id);
        }

        public int AddReview(string userName, int rating, string comment, string title, string imagePath, DateTime dateOfCreation)
        {
            int newId = GetNextId();
            Review review = new Review(newId, userName, rating, comment, title, imagePath, dateOfCreation);
            reviews.Add(review);
            SaveReviewsToXml();
            return newId;
        }

        public void UpdateReview(int id, int newRating, string newComment, string newTitle, string newImagePath)
        {
            var existingReview = GetReviewById(id);
            if (existingReview != null)
            {
                existingReview.Rating = newRating;
                existingReview.Comment = newComment;
                existingReview.Title = newTitle;
                existingReview.ImagePath = newImagePath;
                SaveReviewsToXml();
            }
        }

        public void DeleteReview(int id)
        {
            var reviewToRemove = reviews.FirstOrDefault(r => r.Id == id);
            if (reviewToRemove != null)
            {
                reviews.Remove(reviewToRemove);
                SaveReviewsToXml();
            }
        }

        private int GetNextId()
        {
            return reviews.Count > 0 ? reviews.Max(r => r.Id) + 1 : 1;
        }

        public void ForceReviewSavingToXml()
        {
            SaveReviewsToXml();
        }
    }
}
