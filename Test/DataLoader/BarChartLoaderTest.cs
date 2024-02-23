using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Test.DataLoader
{
    [TestClass]
    public class BarChartLoaderTest
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
        public void LoadDataWithNoParameter()
        {
            // TODO
        }
        #endregion
    }
}