using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness_social_media.MVVM.Model.Repository;

namespace Bussiness_social_media.Services
{
    public interface IReviewService
    {
        List<Review> GetReviews();
        Review GetReviewById(int id);
        int AddReview(string userName, int rating, string comment, string title, string imagePath);
        void UpdateReview(int id, int newRating, string newComment, string newTitle, string newImagePath);
        void DeletePost(int id);
        void LinkAdminCommentIdToReview(int reviewId, int commentId);
    }
    public class ReviewService : IReviewService
    {
        private IReviewRepository reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public int AddReview(string userName, int rating, string comment, string title, string imagePath)
        {
            return reviewRepository.AddReview(userName, rating, comment, title, imagePath, DateTime.Now);
        }

        public void DeletePost(int id)
        {
            reviewRepository.DeleteReview(id);
        }

        public Review GetReviewById(int id)
        {
            return reviewRepository.GetReviewById(id);
        }

        public List<Review> GetReviews()
        {
            return reviewRepository.GetAllReviews();
        }

        public void UpdateReview(int id, int newRating, string newComment, string newTitle, string newImagePath)
        {
            reviewRepository.UpdateReview(id, newRating, newComment, newTitle, newImagePath);
        }

        public void LinkAdminCommentIdToReview(int reviewId, int commentId)
        {
            reviewRepository.GetReviewById(reviewId).AdminCommentId = commentId;
            reviewRepository.ForceReviewSavingToXml();
        }
    }
}
