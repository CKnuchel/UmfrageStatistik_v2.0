using Data.Context;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

[TestClass]
public class ModulRepositoryTest
{
    #region Fields
    private UmfrageContext? _context;
    #endregion

    #region Initialize and Cleanup
    [TestInitialize]
    public void Initialize()
    {
        DbContextOptions options = DbContextTestSetup.CreateUniqueContextOptions();

        _context = DbContextTestSetup.CreateContext(options);
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
        ModulRepository modulRepository = new(_context!);

        // Act
        int nCount = modulRepository.GetAllAsync().Result.Count;

        // Assert
        Assert.AreEqual(3, nCount);
    }
    #endregion
}