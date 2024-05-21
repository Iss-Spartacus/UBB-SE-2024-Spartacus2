using System;
using System.Collections.Generic;
using Moq;
using Bussiness_social_media.MVVM.Model.Repository;
using Bussiness_social_media.Services;
using NUnit.Framework;

namespace Tests.Services
{
    [TestFixture]
    public class ReviewServiceUnitTests
    {
        private Mock<IReviewRepository> mockReviewRepository;
        private IReviewService reviewService;

        [SetUp]
        public void Setup()
        {
            mockReviewRepository = new Mock<IReviewRepository>();
            reviewService = new ReviewService(mockReviewRepository.Object);
        }

        [Test]
        public void GetReviews_ReturnsAllReviews()
        {
            // Arrange
            var reviews = new List<Review> { new Review(), new Review() };
            mockReviewRepository.Setup(repo => repo.GetAllReviews()).Returns(reviews);

            // Act
            var result = reviewService.GetReviews();

            // Assert
            Assert.AreEqual(reviews, result);
        }

        [Test]
        public void GetReviewById_ReturnsReviewWithGivenId()
        {
            // Arrange
            var review = new Review();
            mockReviewRepository.Setup(repo => repo.GetReviewById(It.IsAny<int>())).Returns(review);

            // Act
            var result = reviewService.GetReviewById(1);

            // Assert
            Assert.AreEqual(review, result);
        }

        [Test]
        public void AddReview_ReturnsReviewId()
        {
            // Arrange
            var reviewId = 1;
            mockReviewRepository.Setup(repo => repo.AddReview(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(reviewId);

            // Act
            var result = reviewService.AddReview("userName", 5, "comment", "title", "imagePath");

            // Assert
            Assert.AreEqual(reviewId, result);
        }

        [Test]
        public void UpdateReview_UpdatesReview()
        {
            // Arrange
            mockReviewRepository.Setup(repo => repo.UpdateReview(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // Act
            reviewService.UpdateReview(1, 5, "newComment", "newTitle", "newImagePath");

            // Assert
            mockReviewRepository.Verify(repo => repo.UpdateReview(1, 5, "newComment", "newTitle", "newImagePath"), Times.Once);
        }

        [Test]
        public void DeletePost_DeletesReview()
        {
            // Arrange
            mockReviewRepository.Setup(repo => repo.DeleteReview(It.IsAny<int>()));

            // Act
            reviewService.DeletePost(1);

            // Assert
            mockReviewRepository.Verify(repo => repo.DeleteReview(1), Times.Once);
        }

        [Test]
        public void LinkAdminCommentIdToReview_LinksCommentIdToReview()
        {
            // Arrange
            var review = new Review();
            mockReviewRepository.Setup(repo => repo.GetReviewById(It.IsAny<int>())).Returns(review);
            mockReviewRepository.Setup(repo => repo.ForceReviewSavingToXml());

            // Act
            reviewService.LinkAdminCommentIdToReview(1, 1);

            // Assert
            Assert.AreEqual(1, review.AdminCommentId);
            mockReviewRepository.Verify(repo => repo.ForceReviewSavingToXml(), Times.Once);
        }
    }
}