using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using Bussiness_social_media.MVVM.Model.Repository;

namespace Bussiness_social_media.Tests
{
    [TestFixture]
    public class BusinessRepositoryUnitTests
    {
        private IBusinessRepository businessRepository;
        private Mock<IFileSystem> mockFileSystem;

        [SetUp]
        public void Setup()
        {
            mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
            mockFileSystem.Setup(fs => fs.OpenFile(It.IsAny<string>(), It.IsAny<FileMode>())).Returns(new MemoryStream());
            businessRepository = new BusinessRepository("Assets\\XMLFiles\\businesses.xml");
        }

        [Test]
        public void AddBusiness_ValidInput_AddsBusiness()
        {
            // Arrange
            var name = "Test Business";
            var description = "This is a test business.";
            var category = "Test";
            var logo = "logo.jpg";
            var banner = "banner.jpg";
            var phoneNumber = "123-456-7890";
            var email = "test@test.com";
            var website = "www.test.com";
            var address = "123 Test St.";
            var createdAt = DateTime.Now;
            var managerUsernames = new List<string> { "manager1", "manager2" };
            var postIds = new List<int> { 1, 2, 3 };
            var reviewIds = new List<int> { 1, 2, 3 };
            var faqIds = new List<int> { 1, 2, 3 };

            // Act
            businessRepository.AddBusiness(name, description, category, logo, banner, phoneNumber, email, website, address, createdAt, managerUsernames, postIds, reviewIds, faqIds);

            // Assert
            var allBusinesses = businessRepository.GetAllBusinesses();
            Assert.That(allBusinesses, Has.Exactly(1).Matches<Business>(b => b.Name == name));
        }

        [Test]
        public void GetAllBusinesses_WhenCalled_ReturnsAllBusinesses()
        {
            // Arrange
            // No arrangement necessary for this test

            // Act
            var result = businessRepository.GetAllBusinesses();

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetBusinessById_ValidId_ReturnsExpectedBusiness()
        {
            // Arrange
            var id = 1;
            var expectedBusiness = new Business();
            businessRepository.AddBusiness("Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", DateTime.Now, new List<string>(), new List<int>(), new List<int>(), new List<int>());

            // Act
            var result = businessRepository.GetBusinessById(id);

            // Assert
            Assert.AreEqual(expectedBusiness.Id, result.Id);
        }

        [Test]
        public void UpdateBusiness_ValidInput_UpdatesBusiness()
        {
            // Arrange
            var business = new Business();
            businessRepository.AddBusiness("Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", DateTime.Now, new List<string>(), new List<int>(), new List<int>(), new List<int>());
            business.SetName("Updated");

            // Act
            businessRepository.UpdateBusiness(business);

            // Assert
            Assert.AreEqual("Updated", businessRepository.GetBusinessById(business.Id).Name);
        }

        [Test]
        public void DeleteBusiness_ValidId_DeletesBusiness()
        {
            // Arrange
            businessRepository.AddBusiness("Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", DateTime.Now, new List<string>(), new List<int>(), new List<int>(), new List<int>());
            var id = businessRepository.GetAllBusinesses().Last().Id;
            var expectedCount = businessRepository.GetAllBusinesses().Count - 1;

            // Act
            businessRepository.DeleteBusiness(id);

            // Assert
            Assert.AreEqual(expectedCount, businessRepository.GetAllBusinesses().Count);
        }

        [Test]
        public void SearchBusinesses_ValidKeyword_ReturnsExpectedBusinesses()
        {
            // Arrange
            businessRepository.AddBusiness("Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", DateTime.Now, new List<string>(), new List<int>(), new List<int>(), new List<int>());

            // Act
            var result = businessRepository.SearchBusinesses("Test");

            // Assert
            Assert.IsTrue(result.Any(b => b.Name.Contains("Test")));
        }
    }
}
