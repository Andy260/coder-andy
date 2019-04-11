using System;
using System.Collections;
using CoderAndy.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CoderAndy.Models.Blog.Tests
{
    [TestFixture]
    [Parallelizable]
    [TestOf(typeof(Category))]
    public class CategoryTests
    {
        #region Test Data

        public static IEnumerable SymmetricData
        {
            get
            {
                Category parentCategory     = new Category("Parent Category", null, 10);
                Category firstCategory      = new Category("A Category", parentCategory, 25312);
                Category secondCategory     = new Category("A Category", parentCategory, 25312);
                yield return new TestCaseData(firstCategory, secondCategory)
                    .SetArgDisplayNames("x", "x")
                    .Returns(true);

                yield return new TestCaseData(
                    new Category(
                        name: "Test Category",
                        linkName: "test-category",
                        parent: null,
                        id: 19235),
                    new Category(
                        name: "Another Category",
                        linkName: "another-category",
                        parent: parentCategory,
                        id: 257434))
                    .SetArgDisplayNames("x", "y")
                    .Returns(true);
            }
        }

        public static IEnumerable TransitiveData
        {
            get
            {
                Category parentCategory = new Category("Parent Category", null, 523);
                Category firstCategory = new Category("Test Category", parentCategory, 5423);
                Category secondCategory = new Category("Test Category", parentCategory, 5423);
                Category thirdCategory = new Category("Test Category", parentCategory, 5423);
                yield return new TestCaseData(firstCategory, secondCategory, thirdCategory)
                    .SetArgDisplayNames("x", "x", "x")
                    .Returns(true);

                yield return new TestCaseData(
                    new Category(
                        name: "Test Category",
                        linkName: "test-category",
                        parent: parentCategory,
                        id: 34621),
                    new Category(
                        name: "Another Category",
                        linkName: "another-category",
                        parent: null,
                        id: 34621),
                    new Category(
                        name: "Some Category",
                        linkName: "some-category",
                        parent: parentCategory,
                        id: 34621))
                .SetArgDisplayNames("x", "y", "z")
                .Returns(false);
            }
        }

        #endregion

        #region Constructor Tests

        [Test]
        [Category("Constructor Test")]
        [Description("Tests full constructor of Blog Categories with expected usage")]
        public void FullConstructor()
        {
            int id      = 156;
            string name = "Test Category";
            string link = "test-category";

            // Create Blog Category object
            Category category = new Category(name, link, null, id);

            // Ensure created category object has expected object
            Assert.AreEqual(id, category.Id);
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual(link, category.PermaLink);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests full constructor of Blog Categories with invalid input")]
        [TestCaseSource(typeof(BlogHelperTests.NameToLinkData), "InvalidCharacters")]
        public void FullConstructor_InvalidLinkName(string a_linkName)
        {
            int id      = 10;
            string name = "Test Category";

            // Create Blog Category object
            Category category = new Category(name, a_linkName, null, id);

            // Ensure created category object has expected object
            Assert.AreEqual(id, category.Id);
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual("_", category.PermaLink);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests full constructor of Blog Categories with expected usage with a parent category")]
        public void FullConstructorWithParent()
        {
            int id          = 816;
            string name     = "Test Category";
            string link     = "test-category";
            Category parent = new Category("Test Parent", "test-parent", null, 56);

            // Create Blog Category object
            Category category = new Category(name, link, parent, id);

            // Ensure created category object has expected object
            Assert.AreEqual(id, category.Id);
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual(link, category.PermaLink);
            Assert.AreEqual(parent, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests partial constructor of Blog Categories with expected usage")]
        public void PartialConstructor()
        {
            int id      = 1473;
            string name = "Test Partial Category";

            // Create Blog Category object
            Category category = new Category(name, null, id);

            // Ensure created category object has expected object
            Assert.AreEqual(id, category.Id);
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual("test-partial-category", category.PermaLink);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests partial constructor of Blog Categories with a name which contains invalid characters for link name")]
        [TestCaseSource(typeof(BlogHelperTests.NameToLinkData), "InvalidCharacters")]
        public void PartialConstructor_InvalidName(string a_name)
        {
            int id = 60;

            // Create Blog Category object
            Category category = new Category(a_name, null, id);

            // Ensure created category object has expected object
            Assert.AreEqual(id, category.Id);
            Assert.AreEqual(a_name, category.Name);
            Assert.AreEqual("_", category.PermaLink);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests partial constructor of Blog Categories with expected usage with a parent category")]
        public void PartialConstructorWithParent()
        {
            int id          = 864;
            string name     = "Test Partial Category";
            Category parent = new Category("Test Parent", null, 7);

            // Create Blog Category object
            Category category = new Category(name, parent, id);

            // Ensure created category object has expected object
            Assert.AreEqual(id, category.Id);
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual("test-partial-category", category.PermaLink);
            Assert.AreEqual(parent, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests the minimum constructor of Blog Categories")]
        [TestCase(0)]
        [TestCase(10)]
        [TestCase(int.MaxValue)]
        public void MinConstructor(int a_id)
        {
            Category category = new Category(a_id);

            Assert.AreEqual(a_id, category.Id);
        }

        #endregion

        #region Public Function Tests

        [Test]
        [Category("Function Test")]
        [Description("Tests expected usage of ToString()")]
        public new void ToString()
        {
            Category category = new Category("Test Category", null);

            Assert.AreEqual(category.Name, category.ToString());
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(object) with itself")]
        public void EqualsObject_Reflexive()
        {
            Category category = new Category("Test Category", null, 1215);

            // Reflexive property defined as:
            // x.Equals(x) returns true

            // Ensure repeat calls return the same value
            Assert.Multiple(() =>
            {
                Assert.IsTrue(category.Equals((object)category));
                Assert.IsTrue(category.Equals((object)category));
            });
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(object) with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool EqualsObject_Symmetric(Category firstCategory, Category secondCategory)
        {
            bool xEqualsY = firstCategory.Equals((object)secondCategory);
            bool yEqualsX = secondCategory.Equals((object)firstCategory);

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstCategory.Equals((object)secondCategory));
                Assert.AreEqual(yEqualsX, secondCategory.Equals((object)firstCategory));
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xEqualsY == yEqualsX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(object) for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool EqualsObject_Transitive(Category firstCategory, Category secondCategory, Category thirdCategory)
        {
            bool xEqualsY = firstCategory.Equals((object)secondCategory);
            bool yEqualsZ = secondCategory.Equals((object)thirdCategory);
            bool xEqualsZ = firstCategory.Equals((object)thirdCategory);

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstCategory.Equals((object)secondCategory));
                Assert.AreEqual(yEqualsZ, secondCategory.Equals((object)thirdCategory));
                Assert.AreEqual(xEqualsZ, firstCategory.Equals((object)thirdCategory));
            });

            // Transitive property defined as:
            // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true
            if (xEqualsY && yEqualsZ)
            {
                return xEqualsZ;
            }
            return false;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(object) with NULL as the parameter, and calling object")]
        public void EqualsObject_Null()
        {
            Category category = new Category("Test Category", null, 56767);

            // Ensure checking against NULL returns false
            Assert.Multiple(() =>
            {
                Assert.IsFalse(category.Equals((object)null));
                Assert.IsFalse(category.Equals((object)null));
            });

            // Ensure checking with a null category throws an exception
            Category nullCategory = null;
            Assert.Throws<NullReferenceException>(() => nullCategory.Equals((object)null));
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(category) with itself")]
        public void EqualsCategory_Reflexive()
        {
            Category category = new Category("Test Category", null, 1256);

            // Reflexive property defined as:
            // x.Equals(x) returns true

            // Ensure repeat calls return the same value
            Assert.Multiple(() =>
            {
                Assert.IsTrue(category.Equals(category));
                Assert.IsTrue(category.Equals(category));
            });
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(category) with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool EqualsCategory_Symmetric(Category firstCategory, Category secondCategory)
        {
            bool xEqualsY = firstCategory.Equals(secondCategory);
            bool yEqualsX = secondCategory.Equals(firstCategory);

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstCategory.Equals(secondCategory));
                Assert.AreEqual(yEqualsX, secondCategory.Equals(firstCategory));
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xEqualsY == yEqualsX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(category) for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool EqualsCategory_Transitive(Category firstCategory, Category secondCategory, Category thirdCategory)
        {
            bool xEqualsY = firstCategory.Equals(secondCategory);
            bool yEqualsZ = secondCategory.Equals(thirdCategory);
            bool xEqualsZ = firstCategory.Equals(thirdCategory);

            // Ensure repeat calls returns the same value
            Assert.AreEqual(xEqualsY, firstCategory.Equals(secondCategory));
            Assert.AreEqual(yEqualsZ, secondCategory.Equals(thirdCategory));
            Assert.AreEqual(xEqualsZ, firstCategory.Equals(thirdCategory));

            // Transitive property defined as:
            // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true
            if (xEqualsY && yEqualsZ)
            {
                return xEqualsZ;
            }
            return false;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(category) with NULL as the parameter, and calling object")]
        public void EqualsCategory_Null()
        {
            Category category = new Category("Test Category", null, 7452);

            // Ensure checking against NULL returns false
            Assert.Multiple(() =>
            {
                Assert.IsFalse(category.Equals(null));
                Assert.IsFalse(category.Equals(null));
            });

            // Ensure checking with a null post throws an exception
            Category nullCategory = null;
            Assert.Throws<NullReferenceException>(() => nullCategory.Equals(null));
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of GetHashCode() with itself")]
        public void GetHashCode_Reflexive()
        {
            Category category = new Category("Test Category", null, 94735);

            // Ensure GetHashCode() returns expected value
            int expectedHash    = GenerateHashFromCategory(category);
            int hashCode        = category.GetHashCode();
            Assert.AreEqual(expectedHash, hashCode);

            // Ensure repeat calls return the same value
            Assert.AreEqual(hashCode, category.GetHashCode());
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of GetHashCode() with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool GetHashCode_Symmetric(Category firstCategory, Category secondCategory)
        {
            int firstPostHash   = firstCategory.GetHashCode();
            int secondPostHash  = secondCategory.GetHashCode();

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(firstPostHash, firstCategory.GetHashCode());
                Assert.AreEqual(secondPostHash, secondCategory.GetHashCode());
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return (firstPostHash == secondPostHash) == (secondPostHash == firstPostHash);
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of GetHashCode() for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool GetHashCode_Transitive(Category firstCategory, Category secondCategory, Category thirdCategory)
        {
            int firstPostHash   = firstCategory.GetHashCode();
            int secondPostHash  = secondCategory.GetHashCode();
            int thirdPostHash   = thirdCategory.GetHashCode();

            // Ensure repeat calls returns the same value
            Assert.AreEqual(firstPostHash, firstCategory.GetHashCode());
            Assert.AreEqual(secondPostHash, secondCategory.GetHashCode());
            Assert.AreEqual(thirdPostHash, thirdCategory.GetHashCode());

            // Transitive property defined as:
            // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true
            if ((firstPostHash == secondPostHash) && (secondPostHash == thirdPostHash))
            {
                return firstPostHash == thirdPostHash;
            }
            return false;
        }

        [Test]
        [Category("Property Test")]
        [Description("Tests usage of Uncategorised()")]
        public void Uncategorised()
        {
            // Create in-memory database
            SqliteConnection dbConnection = new SqliteConnection("DataSource=:memory:");
            dbConnection.Open();

            // Create DbContext Options object
            DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(dbConnection)
                    .Options;

            // Apply database migration
            using (ApplicationDbContext context = new ApplicationDbContext(dbOptions))
            {
                context.Database.Migrate();
            }

            // Execute method
            using (ApplicationDbContext context = new ApplicationDbContext(dbOptions))
            {
                Category uncategorised = Category.Uncategorised(context);

                // Ensure output has expected data
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(1, uncategorised.Id);
                    Assert.IsTrue(string.Equals("Uncategorised", uncategorised.Name, StringComparison.Ordinal));
                    Assert.IsTrue(string.Equals("uncategorised", uncategorised.PermaLink, StringComparison.Ordinal));
                    Assert.IsNull(uncategorised.Parent);
                });
            }
        }

        #endregion

        #region Operator Overload Tests

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator with the same object")]
        public void EqualityOperator_Reflexive()
        {
            Category category = new Category("Test Category", null, 23556);

            // Reflexive property defined as:
            // x.Equals(x) returns true

            // Ensure repeat calls return the same value
            Assert.Multiple(() =>
            {
                // Complier will throw a warning when comparing an object with itself for equality
#pragma warning disable CS1718
                Assert.IsTrue(category == category);
                Assert.IsTrue(category == category);
#pragma warning restore CS1718
            });
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool EqualityOperator_Symmetric(Category firstCategory, Category secondCategory)
        {
            bool xEqualsY = firstCategory == secondCategory;
            bool yEqualsX = secondCategory == firstCategory;

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstCategory == secondCategory);
                Assert.AreEqual(yEqualsX, secondCategory == firstCategory);
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xEqualsY == yEqualsX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool EqualityOperator_Transitive(Category firstCategory, Category secondCategory, Category thirdCategory)
        {
            bool xEqualsY = firstCategory == secondCategory;
            bool yEqualsZ = secondCategory == thirdCategory;
            bool xEqualsZ = firstCategory == thirdCategory;

            // Ensure repeat calls returns the same value
            Assert.AreEqual(xEqualsY, firstCategory == secondCategory);
            Assert.AreEqual(yEqualsZ, secondCategory == thirdCategory);
            Assert.AreEqual(xEqualsZ, firstCategory == thirdCategory);

            // Transitive property defined as:
            // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true
            if (xEqualsY && yEqualsZ)
            {
                return xEqualsZ;
            }
            return false;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator with NULL and a valid category object")]
        public void EqualityOperator_Null()
        {
            Category category = new Category("Test Post", null, 45615);

            // Ensure checking against NULL returns false
            Assert.Multiple(() =>
            {
                Assert.IsFalse(category == null);
                Assert.IsFalse(category == null);
            });

            // Ensure checking with a null category throws an exception
            Post nullCategory = null;
            Assert.Throws<NullReferenceException>(() => nullCategory.Equals(null));
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator with the same object")]
        public void InequalityOperator_Reflexive()
        {
            Category category = new Category("Test Category", null, 7815);

            // Reflexive property defined as:
            // x.Equals(x) returns true

            // Ensure repeat calls return the same value
            Assert.Multiple(() =>
            {
                // Complier will throw a warning when comparing an object with itself for equality
#pragma warning disable CS1718
                Assert.IsFalse(category != category);
                Assert.IsFalse(category != category);
#pragma warning restore CS1718
            });
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the inequality operator with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool InequalityOperator_Symmetric(Category firstCategory, Category secondCategory)
        {
            bool xNotEqualToY = firstCategory != secondCategory;
            bool yNotEqualToX = secondCategory != firstCategory;

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xNotEqualToY, firstCategory != secondCategory);
                Assert.AreEqual(yNotEqualToX, secondCategory != firstCategory);
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xNotEqualToY == yNotEqualToX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the inequality operator for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool InequalityOperator_Transitive(Category firstCategory, Category secondCategory, Category thirdCategory)
        {
            bool xNotEqualToY = firstCategory != secondCategory;
            bool yNotEqualToZ = secondCategory != thirdCategory;
            bool xNotEqualToZ = firstCategory != thirdCategory;

            // Ensure repeat calls returns the same value
            Assert.AreEqual(xNotEqualToY, firstCategory != secondCategory);
            Assert.AreEqual(yNotEqualToZ, secondCategory != thirdCategory);
            Assert.AreEqual(xNotEqualToZ, firstCategory != thirdCategory);

            // Transitive property defined as:
            // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true
            if (!xNotEqualToY && !yNotEqualToZ)
            {
                return !xNotEqualToZ;
            }
            return false;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the inequality operator with NULL and a valid category object")]
        public void InequalityOperator_Null()
        {
            Category category = new Category("Test Category", null, 51645);

            // Ensure checking against NULL returns true
            Assert.Multiple(() =>
            {
                Assert.IsTrue(category != null);
                Assert.IsTrue(category != null);
            });

            // Ensure checking with a null category throws an exception
            Category nullCategory = null;
            Assert.Throws<NullReferenceException>(() => nullCategory.Equals(null));
        }

        #endregion

        #region Helper Functions

        private int GenerateHashFromCategory(Category category)
        {
            HashCode hashGenerator = new HashCode();
            hashGenerator.Add(category.Id);
            hashGenerator.Add(category.Name);
            hashGenerator.Add(category.PermaLink);
            hashGenerator.Add(category.Parent);

            return hashGenerator.ToHashCode();
        }

        #endregion
    }
}