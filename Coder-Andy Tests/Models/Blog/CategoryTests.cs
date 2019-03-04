using NUnit.Framework;

namespace CoderAndy.Models.Blog.Tests
{
    [TestFixture]
    [Parallelizable]
    [TestOf(typeof(Category))]
    public class CategoryTests
    {
        #region Constructor Tests

        [Test]
        [Category("Constructor Test")]
        [Description("Tests full constructor of Blog Categories with expected usage")]
        public void FullConstructor()
        {
            string name = "Test Category";
            string link = "test-category";

            // Create Blog Category object
            Category category = new Category(name, link, null);

            // Ensure created category object has expected object
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual(link, category.LinkName);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests full constructor of Blog Categories with invalid input")]
        [TestCaseSource(typeof(BlogHelperTests.NameToLinkData), "InvalidCharacters")]
        public void FullConstructor_InvalidLinkName(string a_linkName)
        {
            string name = "Test Category";

            // Create Blog Category object
            Category category = new Category(name, a_linkName, null);

            // Ensure created category object has expected object
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual("_", category.LinkName);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests full constructor of Blog Categories with expected usage with a parent category")]
        public void FullConstructorWithParent()
        {
            string name     = "Test Category";
            string link     = "test-category";
            Category parent = new Category("Test Parent", "test-parent", null);

            // Create Blog Category object
            Category category = new Category(name, link, parent);

            // Ensure created category object has expected object
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual(link, category.LinkName);
            Assert.AreEqual(parent, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests partial constructor of Blog Categories with expected usage")]
        public void PartialConstructor()
        {
            string name = "Test Partial Category";

            // Create Blog Category object
            Category category = new Category(name, null);

            // Ensure created category object has expected object
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual("test-partial-category", category.LinkName);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests partial constructor of Blog Categories with a name which contains invalid characters for link name")]
        [TestCaseSource(typeof(BlogHelperTests.NameToLinkData), "InvalidCharacters")]
        public void PartialConstructor_InvalidName(string a_name)
        {
            // Create Blog Category object
            Category category = new Category(a_name, null);

            // Ensure created category object has expected object
            Assert.AreEqual(a_name, category.Name);
            Assert.AreEqual("_", category.LinkName);
            Assert.AreEqual(null, category.Parent);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests partial constructor of Blog Categories with expected usage with a parent category")]
        public void PartialConstructorWithParent()
        {
            string name     = "Test Partial Category";
            Category parent = new Category("Test Parent", null);

            // Create Blog Category object
            Category category = new Category(name, parent);

            // Ensure created category object has expected object
            Assert.AreEqual(name, category.Name);
            Assert.AreEqual("test-partial-category", category.LinkName);
            Assert.AreEqual(parent, category.Parent);
        }

        #endregion

        #region Static Property Tests

        [Test]
        [Category("Property Test")]
        [Description("Tests usage of 'Uncategorised' property")]
        public void Uncategorised()
        {
            Assert.AreEqual(0, Category.Uncategorised.Id);
            Assert.AreEqual("Uncategorised", Category.Uncategorised.Name);
            Assert.AreEqual("uncategorised", Category.Uncategorised.LinkName);
            Assert.IsNull(Category.Uncategorised.Parent);
        }

        #endregion
    }
}