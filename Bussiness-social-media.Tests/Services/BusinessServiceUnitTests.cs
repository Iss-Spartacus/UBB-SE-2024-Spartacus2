using NUnit.Framework;
using System;
using System.Collections.Generic;
using Bussiness_social_media.Services;
using Moq;
using Bussiness_social_media.MVVM.Model.Repository;

namespace Bussiness_social_media.Tests
{
    [TestFixture]
    public class BusinessServiceUnitTests
    {
        private Mock<IBusinessRepository> mockBusinessRepository;
        private Mock<IFAQService> mockFaqService;
        private Mock<IPostService> mockPostService;
        private Mock<IReviewService> mockReviewService;
        private Mock<ICommentService> mockCommentService;
        private IBusinessService businessService;

        [SetUp]
        public void Setup()
        {
            mockBusinessRepository = new Mock<IBusinessRepository>();
            mockFaqService = new Mock<IFAQService>();
            mockPostService = new Mock<IPostService>();
            mockReviewService = new Mock<IReviewService>();
            mockCommentService = new Mock<ICommentService>();
            businessService = new BusinessService(mockBusinessRepository.Object, mockFaqService.Object, mockPostService.Object, mockReviewService.Object, mockCommentService.Object);
        }

        [Test]
        public void GetAllBusinesses_WhenCalled_ReturnsAllBusinesses()
        {
            // Arrange
            var expectedBusinesses = new List<Business> { new Business(), new Business() };
            mockBusinessRepository.Setup(repo => repo.GetAllBusinesses()).Returns(expectedBusinesses);

            // Act
            var result = businessService.GetAllBusinesses();

            // Assert
            Assert.AreEqual(expectedBusinesses, result);
        }

        [Test]
        public void GetBusinessById_ValidId_ReturnsExpectedBusiness()
        {
            // Arrange
            var id = 1;
            var expectedBusiness = new Business();
            mockBusinessRepository.Setup(repo => repo.GetBusinessById(id)).Returns(expectedBusiness);

            // Act
            var result = businessService.GetBusinessById(id);

            // Assert
            Assert.AreEqual(expectedBusiness, result);
        }

        [Test]
        public void AddBusiness_ValidInput_AddsBusiness()
        {
            // Arrange
            var name = "Test Business";
            var description = "Test Description";
            var category = "Test Category";
            var logo = "Test Logo";
            var banner = "Test Banner";
            var phoneNumber = "Test Phone Number";
            var email = "Test Email";
            var website = "Test Website";
            var address = "Test Address";
            var createdAt = DateTime.Now;
            var managerUsernames = new List<string> { "Test Manager" };
            var postIds = new List<int> { 1 };
            var reviewIds = new List<int> { 1 };
            var faqIds = new List<int> { 1 };

            // Act
            businessService.AddBusiness(name, description, category, logo, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);

            // Assert
            mockBusinessRepository.Verify(repo => repo.AddBusiness(name, description, category, logo, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds), Times.Once);
        }

        [Test]
        public void UpdateBusiness_ValidInput_UpdatesBusiness()
        {
            // Arrange
            var business = new Business();
            mockBusinessRepository.Setup(repo => repo.UpdateBusiness(business));

            // Act
            businessService.UpdateBusiness(business);

            // Assert
            mockBusinessRepository.Verify(repo => repo.UpdateBusiness(business), Times.Once);
        }

        [Test]
        public void DeleteBusiness_ValidId_DeletesBusiness()
        {
            // Arrange
            var id = 1;
            mockBusinessRepository.Setup(repo => repo.DeleteBusiness(id));

            // Act
            businessService.DeleteBusiness(id);

            // Assert
            mockBusinessRepository.Verify(repo => repo.DeleteBusiness(id), Times.Once);
        }

        [Test]
        public void SearchBusinesses_ValidKeyword_ReturnsExpectedBusinesses()
        {
            // Arrange
            var keyword = "Test";
            var expectedBusinesses = new List<Business> { new Business(), new Business() };
            mockBusinessRepository.Setup(repo => repo.SearchBusinesses(keyword)).Returns(expectedBusinesses);

            // Act
            var result = businessService.SearchBusinesses(keyword);

            // Assert
            Assert.AreEqual(expectedBusinesses, result);
        }

       
        [Test]
        public void GetBusinessesManagedBy_ValidUsername_ReturnsExpectedBusinesses()
        {
            // Arrange
            var username = "Test";
            var expectedBusinesses = new List<Business> { new Business(), new Business() };
            mockBusinessRepository.Setup(repo => repo.GetAllBusinesses()).Returns(expectedBusinesses);

            // Act
            var result = businessService.GetBusinessesManagedBy(username);
            // associate 2 businesses with the username
            expectedBusinesses[0].SetManagerUsernames(new List<string> { username });
            expectedBusinesses[1].SetManagerUsernames(new List<string> { username });

            // Assert
            Assert.AreEqual(expectedBusinesses, result);
        }

        [Test]
        public void IsUserManagerOfBusiness_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            var businessId = 1;
            var username = "Test";
            var expectedBusiness = new Business();
            expectedBusiness.SetManagerUsernames(new List<string> { username });
            mockBusinessRepository.Setup(repo => repo.GetBusinessById(businessId)).Returns(expectedBusiness);

            // Act
            var result = businessService.IsUserManagerOfBusiness(businessId, username);

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void GetAllFAQsOfBusiness_ValidBusinessId_ReturnsExpectedFAQs()
        {
            // Arrange
            var businessId = 1;
            var expectedFAQs = new List<FAQ> { new FAQ(), new FAQ() };
            var business = new Business();
            business.SetFaqIds(new List<int> { 1, 2 });
            mockBusinessRepository.Setup(repo => repo.GetBusinessById(businessId)).Returns(business);
            mockFaqService.Setup(service => service.GetFAQById(It.IsAny<int>())).Returns(new FAQ());

            // Act
            var result = businessService.GetAllFAQsOfBusiness(businessId);

            // Assert
            Assert.AreEqual(expectedFAQs.Count, result.Count);
        }


        [Test]
        public void GetAllReviewsForBusiness_ValidBusinessId_ReturnsExpectedReviews()
        {
            // Arrange
            var businessId = 1;
            var expectedReviews = new List<Review> { new Review(), new Review() };
            var business = new Business();
            business.SetReviewIds(new List<int> { 1, 2 });
            mockBusinessRepository.Setup(repo => repo.GetBusinessById(businessId)).Returns(business);
            mockReviewService.Setup(service => service.GetReviewById(It.IsAny<int>())).Returns(new Review());

            // Act
            var result = businessService.GetAllReviewsForBusiness(businessId);

            // Assert
            Assert.AreEqual(expectedReviews.Count, result.Count);
        }


        [Test]
        public void GetAllPostsOfBusiness_ValidBusinessId_ReturnsExpectedPosts()
        {
            // Arrange
            var businessId = 1;
            var expectedPosts = new List<Post> { new Post(), new Post() };
            var business = new Business();
            business.SetPostIds(new List<int> { 1, 2 });
            mockBusinessRepository.Setup(repo => repo.GetBusinessById(businessId)).Returns(business);
            mockPostService.Setup(service => service.GetPostById(It.IsAny<int>())).Returns(new Post());

            // Act
            var result = businessService.GetAllPostsOfBusiness(businessId);

            // Assert
            Assert.AreEqual(expectedPosts.Count, result.Count);
        }


        [Test]
        public void GetAdminCommentForReview_ValidReviewId_ReturnsExpectedComment()
        {
            // Arrange
            var reviewId = 1;
            var expectedComment = new Comment();
            var review = new Review { AdminCommentId = 1 };
            mockReviewService.Setup(service => service.GetReviewById(reviewId)).Returns(review);
            mockCommentService.Setup(service => service.GetCommentById(It.IsAny<int>())).Returns(expectedComment);

            // Act
            var result = businessService.GetAdminCommentForReview(reviewId);

            // Assert
            Assert.AreEqual(expectedComment, result);
        }

        // Continue with the rest of the methods in the same manner
        // ...


    }
}
