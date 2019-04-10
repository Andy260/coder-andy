using CoderAndy.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CoderAndy.Tests.TestFixtureBases
{
    /// <summary>
    /// Test Fixture base for tests which require a database connection
    /// </summary>
    public class DBUnitFixture
    {
        #region Properties

        /// <summary>
        /// Connection to the database
        /// </summary>
        protected SqliteConnection DbConnection { get; private set; }

        /// <summary>
        /// Database context options object for the active connection
        /// </summary>
        protected DbContextOptions<ApplicationDbContext> DbOptions { get; private set; }

        #endregion

        #region Set-up And Tear-Down

        /// <summary>
        /// One time set-up of the test fixture
        /// </summary>
        [OneTimeSetUp]
        public virtual void OnetimeSetup()
        {
            // Create in-memory database
            DbConnection = new SqliteConnection("DataSource=:memory:");
            DbConnection.Open();

            // Create DbContext Options object
            DbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(DbConnection)
                    .Options;

            // Apply database migration
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                context.Database.Migrate();
            }
        }

        /// <summary>
        /// Run after every test in this fixture
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                // Delete any posts created from the last test run
                context.Database.ExecuteSqlCommand(@"DELETE FROM posts");

                // Delete any categories created from the last test run
                context.Database.ExecuteSqlCommand(@"DELETE FROM categories WHERE Id != 1");
            }
        }

        /// <summary>
        /// One time tear-down of the test fixture
        /// </summary>
        [OneTimeTearDown]
        public virtual void OnetimeTearDown()
        {
            DbConnection.Close();

            DbConnection    = null;
            DbOptions       = null;
        }

        #endregion
    }
}
