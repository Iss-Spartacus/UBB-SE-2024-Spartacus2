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
        private AuthenticationService _authService;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authService = new AuthenticationService(_userRepositoryMock.Object);
        }

        [Test]
        public void AuthenticateUser_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            _userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(true);

            // Act
            bool result = _authService.AuthenticateUser(username, password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AuthenticateUser_WithInvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            _userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(false);

            // Act
            bool result = _authService.AuthenticateUser(username, password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetIsLoggedIn_WhenNotLoggedIn_ReturnsFalse()
        {
            // Act
            bool result = _authService.GetIsLoggedIn();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetIsLoggedIn_WhenLoggedIn_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            _userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(true);
            _authService.AuthenticateUser(username, password);

            // Act
            bool result = _authService.GetIsLoggedIn();

            // Assert
            Assert.That(result, Is.True);
        }


        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<Account> { new Account("user1", "password1"), new Account("user2", "password2") };
            _userRepositoryMock.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            var result = _authService.GetAllUsers();

            // Assert
            Assert.AreEqual(users, result);
        }

        [Test]
        public void LoginStatusChanged_EventIsRaised()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            _userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(true);
            bool eventRaised = false;
            _authService.LoginStatusChanged += (sender, e) => eventRaised = true;

            // Act
            _authService.AuthenticateUser(username, password);

            // Assert
            Assert.IsTrue(eventRaised);
        }
    }
}
