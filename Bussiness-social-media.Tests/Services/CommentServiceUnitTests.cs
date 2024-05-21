using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Bussiness_social_media.Services;
using Bussiness_social_media.MVVM.Model.Repository;

namespace Tests.Services
{
    [TestFixture]
    public class CommentServiceUnitTests
    {
        private CommentService commentService;
        private Mock<ICommentRepository> commentRepositoryMock;

        [SetUp]
        public void Setup()
        {
            commentRepositoryMock = new Mock<ICommentRepository>();
            commentService = new CommentService(commentRepositoryMock.Object);
        }

        [Test]
        public void GetAllComments_WhenCalled_ReturnsAllComments()
        {
            // Arrange
            var comments = new List<Comment> { new Comment(), new Comment() };
            commentRepositoryMock.Setup(repo => repo.GetAllComments()).Returns(comments);

            // Act
            var result = commentService.GetAllComments();

            // Assert
            Assert.AreEqual(comments, result);
        }

        [Test]
        public void GetCommentById_ExistingIdPassed_ReturnsCorrectComment()
        {
            // Arrange
            var comment = new Comment();
            commentRepositoryMock.Setup(repo => repo.GetCommentById(1)).Returns(comment);

            // Act
            var result = commentService.GetCommentById(1);

            // Assert
            Assert.AreEqual(comment, result);
        }

        [Test]
        public void AddComment_ValidCommentPassed_ReturnsCommentId()
        {
            // Arrange
            var commentId = 1;
            commentRepositoryMock.Setup(repo => repo.AddComment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(commentId);

            // Act
            var result = commentService.AddComment("username", "content", DateTime.Now);

            // Assert
            Assert.AreEqual(commentId, result);
        }

        [Test]
        public void UpdateComment_ValidCommentPassed_UpdatesComment()
        {
            // Arrange
            commentRepositoryMock.Setup(repo => repo.UpdateComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            // Act
            commentService.UpdateComment(1, "username", "content", DateTime.Now);

            // Assert
            commentRepositoryMock.Verify(repo => repo.UpdateComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void DeleteComment_ExistingIdPassed_DeletesComment()
        {
            // Arrange
            commentRepositoryMock.Setup(repo => repo.DeleteComment(It.IsAny<int>()));

            // Act
            commentService.DeleteComment(1);

            // Assert
            commentRepositoryMock.Verify(repo => repo.DeleteComment(It.IsAny<int>()), Times.Once);
        }
    }
}