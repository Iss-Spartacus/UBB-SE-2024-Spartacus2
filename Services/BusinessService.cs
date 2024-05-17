
using business_social_media.Services;
using bussiness_social_media.MVVM.Model.Repository;
using System;
using System.IO;
using System.Windows.Media;
using System.Xml.Linq;

namespace bussiness_social_media.Services
{
    public interface IBusinessService
    {
        List<Business> GetAllBusinesses();
        Business GetBusinessById(int id);
        void AddBusiness(string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds);
        void UpdateBusiness(Business business);
        void DeleteBusiness(int id);
        List<Business> SearchBusinesses(string keyword);
        public List<Business> GetBusinessesManagedBy(string username);
        public List<FAQ> GetAllFAQsOfBusiness(int businessID);
        public List<Review> GetAllReviewsForBusiness(int businessId);
        public Comment GetAdminCommentForReview(int reviewId); 
        public void CreateReviewAndAddItToBusiness(int businessId, string userName, int rating, string comment, string title, string imagePath);
        public void CreatePostAndAddItToBusiness(int businessId, string postImagePath, string postCaption);

        public List<Post> GetAllPostsOfBusiness(int businessId);


        public bool IsUserManagerOfBusiness(int businessId, string username);
        public void CreateCommentAndAddItToPost(int postId, string username, string content);

        public List<Comment> GetAllCommentsForPost(int postId);
    }
    public class BusinessService : IBusinessService
    {
        private IBusinessRepository _businessRepository;
        private IFAQService _faqService;
        private IPostService _postService;
        private IReviewService _reviewService;
        private ICommentService _commentService;

        public BusinessService(IBusinessRepository businessRepository, IFAQService FAQService, IPostService postService, IReviewService reviewService, ICommentService commentService)
        {
            _businessRepository = businessRepository;
            _faqService = FAQService;
            _postService = postService;
            _reviewService = reviewService;
            _commentService = commentService;
        }

        ~BusinessService()
        {
            _businessRepository.SaveBusinessesToXml();
        }


        public List<Business> GetAllBusinesses()
        {
            return _businessRepository.GetAllBusinesses();
        }

        public Business GetBusinessById(int id)
        {
            return _businessRepository.GetBusinessById(id);
        }

        public void AddBusiness(string name, string description, string category, string logo, string banner, string phoneNumber, string email, string website, string address, DateTime createdAt, List<string> managerUsernames, List<int> postIds, List<int> reviewIds, List<int> faqIds)
        {
            _businessRepository.AddBusiness(name, description, category, logo, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);
        }

        public void UpdateBusiness(Business business)
        {
            _businessRepository.UpdateBusiness(business);
        }

        public void DeleteBusiness(int id)
        {
            _businessRepository.DeleteBusiness(id);
        }

        public List<Business> SearchBusinesses(string keyword)
        {
           return _businessRepository.SearchBusinesses(keyword);
        }

        public List<Business> GetBusinessesManagedBy(string username)
        {
            List<Business> businessesManagedByUser = new List<Business>();

            foreach (Business business in _businessRepository.GetAllBusinesses())
            {
                if (business.ManagerUsernames.Contains(username))
                {
                    businessesManagedByUser.Add(business);
                }
            }

            return businessesManagedByUser;
        }

        public bool IsUserManagerOfBusiness(int businessId, string username)
        {
            Business business = _businessRepository.GetBusinessById(businessId);
            return business != null && business.ManagerUsernames.Contains(username);
        }

        private void LinkFaqIdToBusiness(int businessId, int faqId)
        {
            Business business = GetBusinessById(businessId);
            business.FaqIds.Add(faqId);
            _businessRepository.SaveBusinessesToXml();
        }

        public void CreateFAQAndAddItToBusiness(int businessId, string faqQuestion, string faqAnswer)
        {
            int faqId = _faqService.AddFAQ(faqQuestion, faqAnswer);
            LinkFaqIdToBusiness(businessId, faqId);
        }

        public List<FAQ> GetAllFAQsOfBusiness(int businessId)
        {
            Business business = GetBusinessById(businessId);
            List<FAQ> givenBusinessFAQs = [];
            foreach (int faqId in business.FaqIds)
            {
                givenBusinessFAQs.Add(_faqService.GetFAQById(faqId));
            }
            return givenBusinessFAQs;
        }

        private void LinkPostIdToBusiness(int businessId, int postId)
        {
            Business business = GetBusinessById(businessId);
            business.PostIds.Add(postId);
            _businessRepository.SaveBusinessesToXml();

        }

        public void CreatePostAndAddItToBusiness(int businessId, string postImagePath, string postCaption)
        {
            int postId = _postService.AddPost(DateTime.Now, postImagePath, postCaption);
            LinkPostIdToBusiness(businessId, postId);
        }

        public List<Post> GetAllPostsOfBusiness(int businessId)
        {
            Business business = GetBusinessById(businessId);
            List<Post> givenBusinessPosts = [];
            foreach (int postId in business.PostIds)
            {
                givenBusinessPosts.Add(_postService.GetPostById(postId));
            }
            return givenBusinessPosts;
        }

        private void LinkReviewIdToBusiness(int businessId, int reviewId)
        {
            Business business = GetBusinessById(businessId);
            business.ReviewIds.Add(reviewId);
            _businessRepository.SaveBusinessesToXml();
        }

        public void CreateReviewAndAddItToBusiness(int businessId, string userName, int rating, string comment, string title, string imagePath)
        {
            int reviewId = _reviewService.AddReview(userName, rating, comment, title, imagePath);
            LinkReviewIdToBusiness(businessId, reviewId);
        }

        public List<Review> GetAllReviewsForBusiness(int businessId)
        {
            Business business = GetBusinessById(businessId);
            List<Review> givenBusinessReviews = [];
            foreach (int reviewId in business.ReviewIds)
            {
                givenBusinessReviews.Add(_reviewService.GetReviewById(reviewId));
            }
            return givenBusinessReviews;
        }

        private int CreateComment(string username, string content)
        {
            return _commentService.AddComment(username, content, DateTime.Now);
        }

        public void CreateCommentAndAddItToPost(int postId, string username, string content)
        {
            int commentId = CreateComment(username, content);
            _postService.LinkCommentIdToPost(postId, commentId);
        }

        public List<Comment> GetAllCommentsForPost(int postId)
        {
            Post post = _postService.GetPostById(postId);
            List<Comment> givenPostComments = [];
            foreach (int commentId in post.CommentIds)
            {
                givenPostComments.Add(_commentService.GetCommentById(commentId));
            }
            return givenPostComments;
        }

        public void CreateAdminCommentAndAddItToReview(int reviewId, string administratorUsername, string content)
        {
            int adminCommentId = CreateComment(administratorUsername, content);
            _reviewService.LinkAdminCommentIdToReview(reviewId, adminCommentId);
        }

        public Comment GetAdminCommentForReview(int reviewId)
        {
            Review review = _reviewService.GetReviewById(reviewId);
            return _commentService.GetCommentById(review.AdminCommentId);
        }
    }
}


