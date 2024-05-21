using NUnit.Framework;
using System.Collections.Generic;
using Bussiness_social_media.Services;
using Moq;
using Bussiness_social_media.MVVM.Model.Repository;

namespace Bussiness_social_media.Tests;

[TestFixture]
public class FAQServiceUnitTests
{
    private Mock<IFAQRepository> mockFAQRepository;
    private IFAQService faqService;

    [SetUp]
    public void Setup()
    {
        mockFAQRepository = new Mock<IFAQRepository>();
        faqService = new FAQService(mockFAQRepository.Object);
    }

    [Test]
    public void AddFAQ_ValidInput_ReturnsExpectedId()
    {
        // Arrange
        var question = "Test Question";
        var answer = "Test Answer";
        var expectedId = 1;
        mockFAQRepository.Setup(repo => repo.AddFAQ(question, answer)).Returns(expectedId);

        // Act
        var result = faqService.AddFAQ(question, answer);

        // Assert
        Assert.AreEqual(expectedId, result);
    }

    [Test]
    public void DeleteFAQ_ValidId_DeletesFAQ()
    {
        // Arrange
        var id = 1;
        mockFAQRepository.Setup(repo => repo.DeleteFAQ(id));

        // Act
        faqService.DeleteFAQ(id);

        // Assert
        mockFAQRepository.Verify(repo => repo.DeleteFAQ(id), Times.Once);
    }

    [Test]
    public void GetAllFAQs_WhenCalled_ReturnsAllFAQs()
    {
        // Arrange
        var expectedFAQs = new List<FAQ> { new FAQ(), new FAQ() };
        mockFAQRepository.Setup(repo => repo.GetAllFAQs()).Returns(expectedFAQs);

        // Act
        var result = faqService.GetAllFAQs();

        // Assert
        Assert.AreEqual(expectedFAQs, result);
    }

    [Test]
    public void GetFAQById_ValidId_ReturnsExpectedFAQ()
    {
        // Arrange
        var id = 1;
        var expectedFAQ = new FAQ();
        mockFAQRepository.Setup(repo => repo.GetFAQById(id)).Returns(expectedFAQ);

        // Act
        var result = faqService.GetFAQById(id);

        // Assert
        Assert.AreEqual(expectedFAQ, result);
    }

    [Test]
    public void UpdateFAQ_ValidInput_UpdatesFAQ()
    {
        // Arrange
        var id = 1;
        var question = "Updated Question";
        var answer = "Updated Answer";
        mockFAQRepository.Setup(repo => repo.UpdateFAQ(id, question, answer));

        // Act
        faqService.UpdateFAQ(id, question, answer);

        // Assert
        mockFAQRepository.Verify(repo => repo.UpdateFAQ(id, question, answer), Times.Once);
    }
}