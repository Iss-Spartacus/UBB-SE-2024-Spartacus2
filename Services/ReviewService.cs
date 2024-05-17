using bussiness_social_media.MVVM.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussiness_social_media.Services
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
        private IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public int AddReview(string userName, int rating, string comment, string title, string imagePath)
        {
            return _reviewRepository.AddReview(userName, rating, comment, title, imagePath, DateTime.Now);
        }

        public void DeletePost(int id)
        {
            _reviewRepository.DeleteReview(id);
        }

        public Review GetReviewById(int id)
        {
            return _reviewRepository.GetReviewById(id);
        }

        public List<Review> GetReviews()
        {
            return _reviewRepository.GetAllReviews();
        }

        public void UpdateReview(int id, int newRating, string newComment, string newTitle, string newImagePath)
        {
            _reviewRepository.UpdateReview(id, newRating, newComment, newTitle, newImagePath);
        }

        public void LinkAdminCommentIdToReview(int reviewId, int commentId)
        {
            _reviewRepository.GetReviewById(reviewId).AdminCommentId = commentId;
            _reviewRepository.ForceReviewSavingToXml();
        }
    }
}
