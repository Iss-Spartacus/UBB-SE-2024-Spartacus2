using System;
using Xunit;
using Moq;
using Bussiness_social_media.Services;
using Bussiness_social_media.MVVM.Model.Repository;

namespace Bussiness_social_media.Tests
{
    public class AuthenticationServiceUnitTests
    {
        private readonly AuthenticationService authService;
        private readonly Mock<IUserRepository> userRepositoryMock;

        public AuthenticationServiceUnitTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            authService = new AuthenticationService(userRepositoryMock.Object);
        }

        [Fact]
        public void AuthenticateUser_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(true);

            // Act
            bool result = authService.AuthenticateUser(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AuthenticateUser_InvalidCredentials_ReturnsFalse()
        {
            // Arrange
            string username = "testUser";
            string password = "testPassword";
            userRepositoryMock.Setup(repo => repo.IsCredentialsValid(username, password)).Returns(false);

            // Act
            bool result = authService.AuthenticateUser(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetIsLoggedIn_Initially_ReturnsFalse()
        {
            // Act
            bool result = authService.GetIsLoggedIn();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetAllUsers_WhenCalled_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<Account> { new Account("user1", "password1"), new Account("user2", "password2") };
            userRepositoryMock.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            var result = authService.GetAllUsers();

            // Assert
            Assert.Equal(users, result);
        }
    }
}
