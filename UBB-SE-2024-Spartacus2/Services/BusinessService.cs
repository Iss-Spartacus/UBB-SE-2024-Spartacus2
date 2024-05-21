using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml.Linq;
using Bussiness_social_media.MVVM.Model.Repository;


namespace Bussiness_social_media.Services
{
    public interface IBusinessService
    {
        List<Business> GetAllBusinesses();
        Business GetBusinessById(int id);
        void AddBusiness(string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds);
        void UpdateBusiness(Business business);
        void DeleteBusiness(int id);
        List<Business> SearchBusinesses(string keyword);
        List<Business> GetBusinessesManagedBy(string username);
        List<FAQ> GetAllFAQsOfBusiness(int businessID);
        List<Review> GetAllReviewsForBusiness(int businessId);
        Comment GetAdminCommentForReview(int reviewId);
        void CreateReviewAndAddItToBusiness(int businessId, string userName, int rating, string comment, string title, string imagePath);
        void CreatePostAndAddItToBusiness(int businessId, string postImagePath, string postCaption);
        List<Post> GetAllPostsOfBusiness(int businessId);
        bool IsUserManagerOfBusiness(int businessId, string username);
        void CreateCommentAndAddItToPost(int postId, string username, string content);
        List<Comment> GetAllCommentsForPost(int postId);
    }

    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository businessRepository;
        private readonly IFAQService faqService;
        private readonly IPostService postService;
        private readonly IReviewService reviewService;
        private readonly ICommentService commentService;

        public BusinessService(IBusinessRepository businessRepository, IFAQService faqService, IPostService postService, IReviewService reviewService, ICommentService commentService)
        {
            this.businessRepository = businessRepository;
            this.faqService = faqService;
            this.postService = postService;
            this.reviewService = reviewService;
            this.commentService = commentService;
        }

        ~BusinessService()
        {
            businessRepository.SaveBusinessesToXml();
        }

        public List<Business> GetAllBusinesses() => businessRepository.GetAllBusinesses();

        public Business GetBusinessById(int id) => businessRepository.GetBusinessById(id);

        public void AddBusiness(string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds) =>
            businessRepository.AddBusiness(name, description, category, logo, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);

        public void UpdateBusiness(Business business) => businessRepository.UpdateBusiness(business);

        public void DeleteBusiness(int id) => businessRepository.DeleteBusiness(id);

        public List<Business> SearchBusinesses(string keyword) => businessRepository.SearchBusinesses(keyword);

        public List<Business> GetBusinessesManagedBy(string username) => businessRepository.GetAllBusinesses().Where(b => b.ManagerUsernames.Contains(username)).ToList();

        public bool IsUserManagerOfBusiness(int businessId, string username) => businessRepository.GetBusinessById(businessId)?.ManagerUsernames.Contains(username) ?? false;

        private void LinkFaqIdToBusiness(int businessId, int faqId)
        {
            Business business = GetBusinessById(businessId);
            business.FaqIds.Add(faqId);
            businessRepository.SaveBusinessesToXml();
        }

        public void CreateFAQAndAddItToBusiness(int businessId, string faqQuestion, string faqAnswer)
        {
            int faqId = faqService.AddFAQ(faqQuestion, faqAnswer);
            LinkFaqIdToBusiness(businessId, faqId);
        }

        public List<FAQ> GetAllFAQsOfBusiness(int businessId)
        {
            Business business = GetBusinessById(businessId);
            return business.FaqIds.Select(faqId => faqService.GetFAQById(faqId)).ToList();
        }

        private void LinkPostIdToBusiness(int businessId, int postId)
        {
            Business business = GetBusinessById(businessId);
            business.PostIds.Add(postId);
            businessRepository.SaveBusinessesToXml();
        }

        public void CreatePostAndAddItToBusiness(int businessId, string postImagePath, string postCaption)
        {
            int postId = postService.AddPost(DateTime.Now, postImagePath, postCaption);
            LinkPostIdToBusiness(businessId, postId);
        }

        public List<Post> GetAllPostsOfBusiness(int businessId)
        {
            Business business = GetBusinessById(businessId);
            return business.PostIds.Select(postId => postService.GetPostById(postId)).ToList();
        }

        private void LinkReviewIdToBusiness(int businessId, int reviewId)
        {
            Business business = GetBusinessById(businessId);
            business.ReviewIds.Add(reviewId);
            businessRepository.SaveBusinessesToXml();
        }

        public void CreateReviewAndAddItToBusiness(int businessId, string userName, int rating, string comment, string title, string imagePath)
        {
            int reviewId = reviewService.AddReview(userName, rating, comment, title, imagePath);
            LinkReviewIdToBusiness(businessId, reviewId);
        }

        public List<Review> GetAllReviewsForBusiness(int businessId)
        {
            Business business = GetBusinessById(businessId);
            return business.ReviewIds.Select(reviewId => reviewService.GetReviewById(reviewId)).ToList();
        }

        private int CreateComment(string username, string content)
        {
            return commentService.AddComment(username, content, DateTime.Now);
        }

        public void CreateCommentAndAddItToPost(int postId, string username, string content)
        {
            int commentId = CreateComment(username, content);
            postService.LinkCommentIdToPost(postId, commentId);
        }

        public List<Comment> GetAllCommentsForPost(int postId)
        {
            Post post = postService.GetPostById(postId);
            return post.CommentIds.Select(commentId => commentService.GetCommentById(commentId)).ToList();
        }

        public void CreateAdminCommentAndAddItToReview(int reviewId, string administratorUsername, string content)
        {
            int adminCommentId = CreateComment(administratorUsername, content);
            reviewService.LinkAdminCommentIdToReview(reviewId, adminCommentId);
        }

        public Comment GetAdminCommentForReview(int reviewId)
        {
            Review review = reviewService.GetReviewById(reviewId);
            return commentService.GetCommentById(review.AdminCommentId);
        }
    }
}

