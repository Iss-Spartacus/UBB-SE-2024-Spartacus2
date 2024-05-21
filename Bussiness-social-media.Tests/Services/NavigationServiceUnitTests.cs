using NUnit.Framework;
using System;
using Bussiness_social_media.Services;
using Moq;
using Bussiness_social_media.Core;

namespace Bussiness_social_media.Tests
{
    [TestFixture]
    public class NavigationServiceUnitTests
    {
        private Mock<Func<Type, TestViewModel>> mockViewModelFactory;
        private INavigationService navigationService;

        [SetUp]
        public void Setup()
        {
            mockViewModelFactory = new Mock<Func<Type, TestViewModel>>();
            navigationService = new NavigationService(mockViewModelFactory.Object);
        }

        [Test]
        public void NavigateTo_ValidViewModel_NavigatesToViewModel()
        {
            // Arrange
            var expectedViewModel = new TestViewModel();
            mockViewModelFactory.Setup(factory => factory(It.IsAny<Type>())).Returns(expectedViewModel);

            // Act
            navigationService.NavigateTo<TestViewModel>();

            // Assert
            Assert.AreEqual(expectedViewModel, navigationService.CurrentView);
        }

        [Test]
        public void BusinessId_SetValidId_ReturnsExpectedId()
        {
            // Arrange
            var expectedId = 1;

            // Act
            navigationService.BusinessId = expectedId;

            // Assert
            Assert.AreEqual(expectedId, navigationService.BusinessId);
        }
    }

    public class TestViewModel : ViewModel
    {
        // This is a concrete subclass of ViewModel for testing purposes.
        // You can add any necessary properties or methods here.
    }
}
