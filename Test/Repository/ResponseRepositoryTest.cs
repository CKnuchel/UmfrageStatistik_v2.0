using Data.Context;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

[TestClass]
public class ResponseRepositoryTest
{
    #region Constants
    private const int UNAVAILABLE_MODUL_ID = 99;
    private const int UNAVAILABLE_QUESTION_ID = 99;
    private const int UNAVAILABLE_ANSWER_ID = 99;
    #endregion

    #region Fields
    private UmfrageContext? _context;
    private ResponseRepository? responseRepository;
    #endregion

    #region Initialize and Cleanup
    [TestInitialize]
    public void Initialize()
    {
        DbContextOptions options = DbContextTestSetup.CreateUniqueContextOptions();
        _context = DbContextTestSetup.CreateContext(options);

        responseRepository = new ResponseRepository(_context);
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
        // Act 
        int nCount = responseRepository!.GetAllAsync().Result.Count;

        // Assert
        Assert.AreEqual(20, nCount);
    }

    [TestMethod]
    public void GetResponseCountByQuestionIdAsyncTest()
    {
        // Act 
        int nResponseCount = responseRepository!.GetResponseCountByQuestionIdAsync(1).Result;

        // Assert
        Assert.AreEqual(6, nResponseCount);
    }

    [TestMethod]
    public void GetResponseCountByNotAvailableQuestionIdAsyncTest()
    {
        // Act 
        int nResponseCount = responseRepository!.GetResponseCountByQuestionIdAsync(UNAVAILABLE_QUESTION_ID).Result;

        // Assert
        Assert.AreEqual(0, nResponseCount);
    }

    [TestMethod]
    public void GetResponseCountByModuleIdAndQuestionIdAsync()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByModuleIdAndQuestionIdAsync(1, 1).Result;

        // Assert
        Assert.AreEqual(4, nCount);
    }

    [TestMethod]
    public void GetResponseCountByModuleIdAndQuestionIdAsyncWithNotAvailableModuleId()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByModuleIdAndQuestionIdAsync(UNAVAILABLE_MODUL_ID, 1).Result;

        // Assert
        Assert.AreEqual(0, nCount);
    }

    [TestMethod]
    public void GetResponseCountByModuleIdAndQuestionIdAsyncWithNotAvailableQuestionId()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByModuleIdAndQuestionIdAsync(1, UNAVAILABLE_QUESTION_ID).Result;

        // Assert
        Assert.AreEqual(0, nCount);
    }

    [TestMethod]
    public void GetResponseCountByAnswerIdAsync()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByAnswerIdAsync(1).Result;

        // Assert
        Assert.AreEqual(2, nCount);
    }

    [TestMethod]
    public void GetResponseCountByAnswerIdAsyncWithUnavailableAnswerId()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByAnswerIdAsync(UNAVAILABLE_ANSWER_ID).Result;

        // Assert
        Assert.AreEqual(0, nCount);
    }

    [TestMethod]
    public void GetResponseCountByAnswerIdAndModulIdTest()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByAnswerIdAndModulId(2, 4).Result;

        // Assert
        Assert.AreEqual(1, nCount);
    }

    [TestMethod]
    public void GetResponseCountByAnswerIdAndModulIdTestWithUnavailableAnswerId()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByAnswerIdAndModulId(2, UNAVAILABLE_ANSWER_ID).Result;

        // Assert
        Assert.AreEqual(0, nCount);
    }

    [TestMethod]
    public void GetResponseCountByAnswerIdAndModulIdTestWithUnavailableModulId()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByAnswerIdAndModulId(UNAVAILABLE_MODUL_ID, 4).Result;

        // Assert
        Assert.AreEqual(0, nCount);
    }

    [TestMethod]
    public void GetResponseCountByQuestionIdAndValueTest()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByQuestionIdAndValue(1, 1).Result;

        // Assert
        Assert.AreEqual(6, nCount);
    }

    [TestMethod]
    public void GetResponseCountByQuestionIdAndValueTestWithQuestionType2()
    {
        // Act
        int nCount = responseRepository!.GetResponseCountByQuestionIdAndValue(4, 10).Result;

        // Assert
        Assert.AreEqual(2, nCount);
    }
    #endregion
}