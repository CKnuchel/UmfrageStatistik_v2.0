using Data.Context;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

[TestClass]
public class QuestionRepositoryTest
{
    #region Fields
    private UmfrageContext? _context;
    private QuestionRepository? questionRepository;
    #endregion

    #region Initialize and Cleanup
    [TestInitialize]
    public void Initialize()
    {
        DbContextOptions options = DbContextTestSetup.CreateUniqueContextOptions();
        _context = DbContextTestSetup.CreateContext(options);

        questionRepository = new QuestionRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        DbContextTestSetup.DestroyContext(_context);
    }
    #endregion

    [TestMethod]
    public void GetAllAsyncTest()
    {
        // Arrange

        // Act
        int nCount = questionRepository!.GetAllAsync().Result.Count;

        // Assert
        Assert.AreEqual(4, nCount);
    }
}