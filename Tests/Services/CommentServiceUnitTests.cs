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
        private CommentService _commentService;
        private Mock<ICommentRepository> _commentRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _commentService = new CommentService(_commentRepositoryMock.Object);
        }

        [Test]
        public void GetAllComments_WhenCalled_ReturnsAllComments()
        {
            // Arrange
            var comments = new List<Comment> { new Comment(), new Comment() };
            _commentRepositoryMock.Setup(repo => repo.GetAllComments()).Returns(comments);

            // Act
            var result = _commentService.GetAllComments();

            // Assert
            Assert.AreEqual(comments, result);
        }

        [Test]
        public void GetCommentById_ExistingIdPassed_ReturnsCorrectComment()
        {
            // Arrange
            var comment = new Comment();
            _commentRepositoryMock.Setup(repo => repo.GetCommentById(1)).Returns(comment);

            // Act
            var result = _commentService.GetCommentById(1);

            // Assert
            Assert.AreEqual(comment, result);
        }

        [Test]
        public void AddComment_ValidCommentPassed_ReturnsCommentId()
        {
            // Arrange
            var commentId = 1;
            _commentRepositoryMock.Setup(repo => repo.AddComment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(commentId);

            // Act
            var result = _commentService.AddComment("username", "content", DateTime.Now);

            // Assert
            Assert.AreEqual(commentId, result);
        }

        [Test]
        public void UpdateComment_ValidCommentPassed_UpdatesComment()
        {
            // Arrange
            _commentRepositoryMock.Setup(repo => repo.UpdateComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            // Act
            _commentService.UpdateComment(1, "username", "content", DateTime.Now);

            // Assert
            _commentRepositoryMock.Verify(repo => repo.UpdateComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void DeleteComment_ExistingIdPassed_DeletesComment()
        {
            // Arrange
            _commentRepositoryMock.Setup(repo => repo.DeleteComment(It.IsAny<int>()));

            // Act
            _commentService.DeleteComment(1);

            // Assert
            _commentRepositoryMock.Verify(repo => repo.DeleteComment(It.IsAny<int>()), Times.Once);
        }
    }
}
