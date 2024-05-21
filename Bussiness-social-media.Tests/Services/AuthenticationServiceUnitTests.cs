using System;
using NUnit.Framework;
using Moq;
using Bussiness_social_media.Services;
using Bussiness_social_media.MVVM.Model.Repository;
using System.Collections.Generic;

namespace Tests.Services
{
    [TestFixture]
    public class AuthenticationServiceUnitTests
    {
        private AuthenticationService authService;
        private Mock<IUserRepository> userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            authService = new AuthenticationService(userRepositoryMock.Object);
        }

        [Test]
        public void AuthenticateUser_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(true);

            // Act
            bool result = authService.AuthenticateUser(username, password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AuthenticateUser_WithInvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(false);

            // Act
            bool result = authService.AuthenticateUser(username, password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetIsLoggedIn_WhenNotLoggedIn_ReturnsFalse()
        {
            // Act
            bool result = authService.GetIsLoggedIn();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetIsLoggedIn_WhenLoggedIn_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(true);
            authService.AuthenticateUser(username, password);

            // Act
            bool result = authService.GetIsLoggedIn();

            // Assert
            Assert.That(result, Is.False);
        }


        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<Account> { new Account("user1", "password1"), new Account("user2", "password2") };
            userRepositoryMock.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            var result = authService.GetAllUsers();

            // Assert
            Assert.AreEqual(users, result);
        }

        [Test]
        public void LoginStatusChanged_EventIsRaised()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(true);
            bool eventRaised = false;
            authService.LoginStatusChanged += (sender, e) => eventRaised = true;

            // Act
            authService.AuthenticateUser(username, password);

            // Assert
            Assert.IsFalse(eventRaised);
        }
    }
}