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
        protected static SqliteConnection DbConnection { get; set; }

        /// <summary>
        /// Database context options object for the active connection
        /// </summary>
        protected static DbContextOptions<ApplicationDbContext> DbOptions { get; set; }

        #endregion

        #region Set-up And Tear-Down

        /// <summary>
        /// One time set-up of the test fixture
        /// </summary>
        [OneTimeSetUp]
        protected virtual void OnetimeSetup()
        {
            if (DbOptions != null &&
                DbConnection != null)
            {
                return;
            }

            SqliteConnection dbConnection;
            DbContextOptions<ApplicationDbContext> dbOptions;
            CreateDatabase(out dbConnection, out dbOptions);

            DbConnection    = dbConnection;
            DbOptions       = DbOptions;
        }

        /// <summary>
        /// Run after every test in this fixture
        /// </summary>
        [TearDown]
        protected virtual void TearDown()
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
        protected virtual void OnetimeTearDown()
        {
            if (DbConnection == null)
            {
                return;
            }

            DestroyDatabase(DbConnection);

            DbConnection    = null;
            DbOptions       = null;
        }

        #endregion

        #region Public Functions

        protected static void CreateDatabase(out SqliteConnection connection, out DbContextOptions<ApplicationDbContext> options)
        {
            // Create in-memory database
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Create DbContext Options object
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;

            // Apply database migration
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                context.Database.Migrate();
            }
        }

        protected static void DestroyDatabase(SqliteConnection connection)
        {
            if (connection == null)
            {
                throw new System.ArgumentNullException(nameof(connection));
            }

            connection.Close();
        }

        #endregion
    }
}
