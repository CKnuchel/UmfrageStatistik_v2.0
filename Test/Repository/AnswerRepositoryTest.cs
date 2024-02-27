using Common.Models;
using Data.Context;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

[TestClass]
public class AnswerRepositoryTest
{
    #region Fields
    private UmfrageContext? _context;
    private AnswerRepository? answerRepository;
    #endregion

    #region Initialize and Cleanup
    [TestInitialize]
    public void Initialize()
    {
        DbContextOptions options = DbContextTestSetup.CreateUniqueContextOptions();

        _context = DbContextTestSetup.CreateContext(options);

        answerRepository = new AnswerRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        DbContextTestSetup.DestroyContext(_context);
    }
    #endregion

    #region Tests
    [TestMethod]
    public void GetAllAsyncTest()
    {
        // Arrange

        // Act
        int nCount = answerRepository.GetAllAsync().Result.Count;

        // Assert
        Assert.AreEqual(8, nCount);
    }

    [TestMethod]
    public void GetByQuestionIdTest()
    {
        // Arrange 
        int nQuestionId = 2;

        // Act
        List<Answer> answers = answerRepository.GetByQuestionId(nQuestionId).Result;

        // Assert
        Assert.AreEqual(3, answers.Count);
        Assert.AreEqual("Die Dauer war angemessen.", answers[0].Text);
    }

    [TestMethod]
    public void GetByQuestionIdWithNotExistingValueTest()
    {
        // Arrange 
        int nQuestionId = 99;

        // Act
        int nCount = answerRepository.GetByQuestionId(nQuestionId).Result.Count;

        // Assert
        Assert.AreEqual(0, nCount);
    }
    #endregion
}