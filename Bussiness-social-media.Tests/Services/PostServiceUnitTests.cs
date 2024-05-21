using NUnit.Framework;
using System;
using System.Collections.Generic;
using Bussiness_social_media.Services;
using Moq;
using Bussiness_social_media.MVVM.Model.Repository;

namespace Bussiness_social_media.Tests
{
    [TestFixture]
    public class PostServiceUnitTests
    {
        private Mock<IPostRepository> mockPostRepository;
        private IPostService postService;

        [SetUp]
        public void Setup()
        {
            mockPostRepository = new Mock<IPostRepository>();
            postService = new PostService(mockPostRepository.Object);
        }

        [Test]
        public void GetAllPosts_WhenCalled_ReturnsAllPosts()
        {
            // Arrange
            var expectedPosts = new List<Post> { new Post(), new Post() };
            mockPostRepository.Setup(repo => repo.GetAllPosts()).Returns(expectedPosts);

            // Act
            var result = postService.GetAllPosts();

            // Assert
            Assert.AreEqual(expectedPosts, result);
        }

        [Test]
        public void GetPostById_ValidId_ReturnsExpectedPost()
        {
            // Arrange
            var id = 1;
            var expectedPost = new Post();
            mockPostRepository.Setup(repo => repo.GetPostById(id)).Returns(expectedPost);

            // Act
            var result = postService.GetPostById(id);

            // Assert
            Assert.AreEqual(expectedPost, result);
        }

        [Test]
        public void AddPost_ValidInput_AddsPost()
        {
            // Arrange
            var creationDate = DateTime.Now;
            var imagePath = "Test Image Path";
            var caption = "Test Caption";
            var expectedPostId = 1;
            mockPostRepository.Setup(repo => repo.AddPost(creationDate, imagePath, caption)).Returns(expectedPostId);

            // Act
            var result = postService.AddPost(creationDate, imagePath, caption);

            // Assert
            Assert.AreEqual(expectedPostId, result);
        }

        [Test]
        public void UpdatePost_ValidInput_UpdatesPost()
        {
            // Arrange
            var id = 1;
            var newCreationDate = DateTime.Now;
            var newImagePath = "New Image Path";
            var newCaption = "New Caption";
            var postToUpdate = new Post();
            mockPostRepository.Setup(repo => repo.GetPostById(id)).Returns(postToUpdate);

            // Act
            postService.UpdatePost(id, newCreationDate, newImagePath, newCaption);

            // Assert
            mockPostRepository.Verify(repo => repo.UpdatePost(postToUpdate), Times.Once);
        }

        [Test]
        public void DeletePost_ValidId_DeletesPost()
        {
            // Arrange
            var id = 1;

            // Act
            postService.DeletePost(id);

            // Assert
            mockPostRepository.Verify(repo => repo.DeletePost(id), Times.Once);
        }

        [Test]
        public void LinkCommentIdToPost_ValidInput_LinksCommentToPost()
        {
            // Arrange
            var postId = 1;
            var commentId = 1;
            var postToCommentOn = new Post();
            mockPostRepository.Setup(repo => repo.GetPostById(postId)).Returns(postToCommentOn);

            // Act
            postService.LinkCommentIdToPost(postId, commentId);

            // Assert
            mockPostRepository.Verify(repo => repo.ForcePostSavingToXml(), Times.Once);
        }
    }
}
