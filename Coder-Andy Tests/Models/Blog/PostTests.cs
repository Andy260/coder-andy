using System;
using System.Collections;
using CoderAndy.Data;
using CoderAndy.Tests.TestFixtureBases;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CoderAndy.Models.Blog.Tests
{
    [TestFixture]
    [Parallelizable]
    [TestOf(typeof(Post))]
    public class PostTests : DBUnitFixture
    {
        #region Test Data

        public static IEnumerable SymmetricData
        {
            get
            {
                // Ensure we have a database context
                if (DbOptions == null)
                {
                    SqliteConnection dbConnection;
                    DbContextOptions<ApplicationDbContext> dbOptions;
                    CreateDatabase(out dbConnection, out dbOptions);

                    DbConnection    = dbConnection;
                    DbOptions       = dbOptions;
                }

                using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
                {
                    Post firstPost  = new Post(context, "A Post", "<p>Post content.</p>", 55);
                    Post secondPost = new Post(context, "A Post", "<p>Post content.</p>", 55);

                    yield return new TestCaseData(firstPost, secondPost)
                        .SetArgDisplayNames("x", "x")
                        .Returns(true);

                    yield return new TestCaseData(
                    new Post(
                        context: context,
                        title: "Test Post",
                        link: "test-post",
                        publishTime: DateTime.Today,
                        category: Category.Uncategorised(context),
                        content: "<p>Post Content.</p>",
                        description: "A test post.",
                        id: 19235),
                    new Post(
                        context: context,
                        title: "Another Test Post",
                        link: "another-test-post",
                        publishTime: DateTime.Today,
                        category: Category.Uncategorised(context),
                        content: "<p>Post Content.</p>",
                        description: "Another test post.",
                        id: 100))
                    .SetArgDisplayNames("x", "y")
                    .Returns(true);
                }
            }
        }

        public static IEnumerable TransitiveData
        {
            get
            {
                // Ensure we have a database context
                if (DbOptions == null)
                {
                    SqliteConnection dbConnection;
                    DbContextOptions<ApplicationDbContext> dbOptions;
                    CreateDatabase(out dbConnection, out dbOptions);

                    DbConnection    = dbConnection;
                    DbOptions       = dbOptions;
                }

                using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
                {
                    Post firstPost  = new Post(context, "Test Post", "<p>Post content.</p>", 55);
                    Post secondPost = new Post(context, "Test Post", "<p>Post content.</p>", 55);
                    Post thirdPost  = new Post(context, "Test Post", "<p>Post content.</p>", 55);
                    yield return new TestCaseData(firstPost, secondPost, thirdPost)
                        .SetArgDisplayNames("x", "x", "x")
                        .Returns(true);

                    yield return new TestCaseData(
                        new Post(
                            context: context,
                            title: "Test Post",
                            link: "test-post",
                            publishTime: DateTime.Today,
                            category: Category.Uncategorised(context),
                            content: "<p>Post Content.</p>",
                            description: "A test post.",
                            id: 35634),
                        new Post(
                            context: context,
                            title: "Another Test Post",
                            link: "another-test-post",
                            publishTime: DateTime.Today,
                            category: Category.Uncategorised(context),
                            content: "<p>Post Content.</p>",
                            description: "Another test post.",
                            id: 26948),
                        new Post(
                            context: context,
                            title: "Some Test Post",
                            link: "some-test-post",
                            publishTime: DateTime.Today,
                            category: Category.Uncategorised(context),
                            content: "<p>A bunch of post Content.</p>",
                            description: "Some test post.",
                            id: 15))
                    .SetArgDisplayNames("x", "y", "z")
                    .Returns(false);
                }
            }
        }

        public static IEnumerable FullConstructorData
        {
            get
            {
                int id                  = 15429;
                string title            = "Test Post";
                string link             = "test-post";
                DateTime publishTime    = new DateTime(2019, 3, 1);
                Category category       = new Category("Test Category", null);
                string content          = "<p>Test Content</p>";
                string description      = "Test Post description";

                yield return new TestCaseData(id, title, link, publishTime, category, content, description);

                id                      = 4561;
                title                   = "A Test Post";
                link                    = "a-test-post";
                publishTime             = new DateTime(2018, 6, 3);
                category                  = null;
                content                 = "<p>The Post Content</p>";
                description             = "A Test Post description.";

                yield return new TestCaseData(id, title, link, publishTime, category, content, description);
            }
        }

        #endregion

        #region Constructor Tests

        [Test]
        [Category("Constructor Test")]
        [Description("Tests expected usage of the full constructor")]
        [TestCaseSource(nameof(FullConstructorData))]
        public void FullConstructor(int id, string title, string link, DateTime publishTime, Category category, string content, string description)
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, title, link, publishTime, category, content, description, id);

                // Ensure created post object has expected values
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(id, post.Id);
                    Assert.AreEqual(title, post.Title);
                    Assert.AreEqual(link, post.Link);
                    Assert.AreEqual(DateTime.Today, post.CreationTime);
                    Assert.AreEqual(DateTime.Today, post.LastModificationTime);
                    Assert.AreEqual(publishTime, post.PublishTime);
                    Assert.AreEqual(category ?? Category.Uncategorised(context), post.Category);
                    Assert.AreEqual(content, post.Content);
                    Assert.AreEqual(description, post.Description);
                });
            }
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests expected usage of the partial constructor")]
        public void PartialConstructor()
        {
            int id              = 10;
            string title        = "Test Post";
            Category category   = new Category("Test Category", null);
            string content      = "<p>Test Content</p>";

            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, title, category, content, id);

                // Ensure created post object has expected values
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(id, post.Id);
                    Assert.AreEqual(title, post.Title);
                    Assert.AreEqual("test-post", post.Link);
                    Assert.AreEqual(DateTime.Today, post.CreationTime);
                    Assert.AreEqual(DateTime.Today, post.LastModificationTime);
                    Assert.AreEqual(DateTime.Today, post.PublishTime);
                    Assert.AreEqual(category, post.Category);
                    Assert.AreEqual(content, post.Content);
                    Assert.IsNull(post.Description);
                });
            }
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests expected usage of partial-minimum constructor")]
        public void PartialMinConstructor()
        {
            int id          = 15734;
            string title    = "Another Test Post";
            string content  = "<h1>More Test Content</h1>";

            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, title, content, id);

                // Ensure created post object has expected values
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(id, post.Id);
                    Assert.AreEqual(title, post.Title);
                    Assert.AreEqual("another-test-post", post.Link);
                    Assert.AreEqual(DateTime.Today, post.CreationTime);
                    Assert.AreEqual(DateTime.Today, post.LastModificationTime);
                    Assert.AreEqual(DateTime.Today, post.PublishTime);
                    Assert.AreEqual(Category.Uncategorised(context), post.Category);
                    Assert.AreEqual(content, post.Content);
                    Assert.IsTrue(string.IsNullOrEmpty(post.Description));
                });
            }
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests expected usage of the minimum constructor")]
        public void MinConstructor()
        {
            int id      = 56;

            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, id);

                // Ensure created post object has expected values
                Assert.Multiple(() =>
                {
                    Assert.AreEqual(id, post.Id);
                    Assert.IsTrue(string.IsNullOrEmpty(post.Title));
                    Assert.AreEqual("_", post.Link);
                    Assert.AreEqual(DateTime.Today, post.CreationTime);
                    Assert.AreEqual(DateTime.Today, post.LastModificationTime);
                    Assert.AreEqual(DateTime.Today, post.PublishTime);
                    Assert.AreEqual(Category.Uncategorised(context), post.Category);
                    Assert.IsTrue(string.IsNullOrEmpty(post.Content));
                    Assert.IsTrue(string.IsNullOrEmpty(post.Description));
                });
            }
        }

        #endregion

        #region Public Function Tests

        [Test]
        [Category("Function Test")]
        [Description("Tests expected usage of IsPublished()")]
        public void IsPublished()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                // Create Post object
                DateTime publishTime = DateTime.UtcNow - TimeSpan.FromDays(1);
                Post post = new Post(context, "Another Test Post", "another-test-post", publishTime, null, null, null, 147654);

                Assert.Multiple(() =>
                {
                    // Ensure posts published in the past are considered published
                    Assert.IsTrue(post.IsPublished(DateTime.UtcNow));

                    // Ensure posts published in the future aren't considered published
                    post.PublishTime += TimeSpan.FromDays(2);
                    Assert.IsFalse(post.IsPublished(DateTime.UtcNow));
                });
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests expected usage of ToString()")]
        public new void ToString()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Content.</p>");

                Assert.AreEqual(post.Title, post.ToString());
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(object) with itself")]
        public void EqualsObject_Reflexive()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Reflexive property defined as:
                // x.Equals(x) returns true

                // Ensure repeat calls return the same value
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(post.Equals((object)post));
                    Assert.IsTrue(post.Equals((object)post));
                });
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(object) with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool EqualsObject_Symmetric(Post firstPost, Post secondPost)
        {
            bool xEqualsY = firstPost.Equals((object)secondPost);
            bool yEqualsX = secondPost.Equals((object)firstPost);

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstPost.Equals((object)secondPost));
                Assert.AreEqual(yEqualsX, secondPost.Equals((object)firstPost));
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xEqualsY == yEqualsX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(object) for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool EqualsObject_Transitive(Post firstPost, Post secondPost, Post thirdPost)
        {
            bool xEqualsY = firstPost.Equals((object)secondPost);
            bool yEqualsZ = secondPost.Equals((object)thirdPost);
            bool xEqualsZ = firstPost.Equals((object)thirdPost);

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstPost.Equals((object)secondPost));
                Assert.AreEqual(yEqualsZ, secondPost.Equals((object)thirdPost));
                Assert.AreEqual(xEqualsZ, firstPost.Equals((object)thirdPost));
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
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Ensure checking against NULL returns false
                Assert.Multiple(() =>
                {
                    Assert.IsFalse(post.Equals((object)null));
                    Assert.IsFalse(post.Equals((object)null));
                });

                // Ensure checking with a null post throws an exception
                Post nullPost = null;
                Assert.Throws<NullReferenceException>(() => nullPost.Equals((object)null));
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(post) with itself")]
        public void EqualsPost_Reflexive()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Reflexive property defined as:
                // x.Equals(x) returns true

                // Ensure repeat calls return the same value
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(post.Equals(post));
                    Assert.IsTrue(post.Equals(post));
                });
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(post) with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool EqualsPost_Symmetric(Post firstPost, Post secondPost)
        {
            bool xEqualsY = firstPost.Equals(secondPost);
            bool yEqualsX = secondPost.Equals(firstPost);

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstPost.Equals(secondPost));
                Assert.AreEqual(yEqualsX, secondPost.Equals(firstPost));
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xEqualsY == yEqualsX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of Equals(post) for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool EqualsPost_Transitive(Post firstPost, Post secondPost, Post thirdPost)
        {
            bool xEqualsY = firstPost.Equals(secondPost);
            bool yEqualsZ = secondPost.Equals(thirdPost);
            bool xEqualsZ = firstPost.Equals(thirdPost);

            // Ensure repeat calls returns the same value
            Assert.AreEqual(xEqualsY, firstPost.Equals(secondPost));
            Assert.AreEqual(yEqualsZ, secondPost.Equals(thirdPost));
            Assert.AreEqual(xEqualsZ, firstPost.Equals(thirdPost));

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
        [Description("Tests usage of Equals(post) with NULL as the parameter, and calling object")]
        public void EqualsPost_Null()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Ensure checking against NULL returns false
                Assert.Multiple(() =>
                {
                    Assert.IsFalse(post.Equals(null));
                    Assert.IsFalse(post.Equals(null));
                });

                // Ensure checking with a null post throws an exception
                Post nullPost = null;
                Assert.Throws<NullReferenceException>(() => nullPost.Equals(null));
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of GetHashCode() with itself")]
        public void GetHashCode_Reflexive()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Ensure GetHashCode() returns expected value
                int expectedHash = GenerateHashFromPost(post);
                int hashCode = post.GetHashCode();
                Assert.AreEqual(expectedHash, hashCode);

                // Ensure repeat calls return the same value
                Assert.AreEqual(hashCode, post.GetHashCode());
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of GetHashCode() with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool GetHashCode_Symmetric(Post firstPost, Post secondPost)
        {
            int firstPostHash   = firstPost.GetHashCode();
            int secondPostHash  = secondPost.GetHashCode();

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(firstPostHash, firstPost.GetHashCode());
                Assert.AreEqual(secondPostHash, secondPost.GetHashCode());
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return (firstPostHash == secondPostHash) == (secondPostHash == firstPostHash);
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of GetHashCode() for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool GetHashCode_Transitive(Post firstPost, Post secondPost, Post thirdPost)
        {
            int firstPostHash   = firstPost.GetHashCode();
            int secondPostHash  = secondPost.GetHashCode();
            int thirdPostHash   = thirdPost.GetHashCode();

            // Ensure repeat calls returns the same value
            Assert.AreEqual(firstPostHash, firstPost.GetHashCode());
            Assert.AreEqual(secondPostHash, secondPost.GetHashCode());
            Assert.AreEqual(thirdPostHash, thirdPost.GetHashCode());

            // Transitive property defined as:
            // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true
            if ((firstPostHash == secondPostHash) && (secondPostHash == thirdPostHash))
            {
                return firstPostHash == thirdPostHash;
            }
            return false;
        }

        #endregion

        #region Operator Overload Tests

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator with the same object")]
        public void EqualityOperator_Reflexive()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Reflexive property defined as:
                // x.Equals(x) returns true

                // Ensure repeat calls return the same value
                Assert.Multiple(() =>
                {
                    // Complier will throw a warning when comparing an object with itself for equality
#pragma warning disable CS1718
                    Assert.IsTrue(post == post);
                    Assert.IsTrue(post == post);
#pragma warning restore CS1718
                });
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool EqualityOperator_Symmetric(Post firstPost, Post secondPost)
        {
            bool xEqualsY = firstPost == secondPost;
            bool yEqualsX = secondPost == firstPost;

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xEqualsY, firstPost == secondPost);
                Assert.AreEqual(yEqualsX, secondPost == firstPost);
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xEqualsY == yEqualsX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool EqualityOperator_Transitive(Post firstPost, Post secondPost, Post thirdPost)
        {
            bool xEqualsY = firstPost == secondPost;
            bool yEqualsZ = secondPost == thirdPost;
            bool xEqualsZ = firstPost == thirdPost;

            // Ensure repeat calls returns the same value
            Assert.AreEqual(xEqualsY, firstPost == secondPost);
            Assert.AreEqual(yEqualsZ, secondPost == thirdPost);
            Assert.AreEqual(xEqualsZ, firstPost == thirdPost);

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
        [Description("Tests usage of the equality operator with NULL and a valid post object")]
        public void EqualityOperator_Null()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Ensure checking against NULL returns false
                Assert.Multiple(() =>
                {
                    Assert.IsFalse(post == null);
                    Assert.IsFalse(post == null);
                });

                // Ensure checking with a null post throws an exception
                Post nullPost = null;
                Assert.Throws<NullReferenceException>(() => nullPost.Equals(null));
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the equality operator with the same object")]
        public void InequalityOperator_Reflexive()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Reflexive property defined as:
                // x.Equals(x) returns true

                // Ensure repeat calls return the same value
                Assert.Multiple(() =>
                {
                    // Complier will throw a warning when comparing an object with itself for equality
#pragma warning disable CS1718
                    Assert.IsFalse(post != post);
                    Assert.IsFalse(post != post);
#pragma warning restore CS1718
                });
            }
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the inequality operator with different objects")]
        [TestCaseSource(nameof(SymmetricData))]
        public bool InequalityOperator_Symmetric(Post firstPost, Post secondPost)
        {
            bool xNotEqualToY = firstPost != secondPost;
            bool yNotEqualToX = secondPost != firstPost;

            // Ensure repeat calls returns the same value
            Assert.Multiple(() =>
            {
                Assert.AreEqual(xNotEqualToY, firstPost != secondPost);
                Assert.AreEqual(yNotEqualToX, secondPost != firstPost);
            });

            // Symmetric property defined as:
            // x.Equals(y) returns the same value as y.Equals(x)
            return xNotEqualToY == yNotEqualToX;
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests usage of the inequality operator for the transitive property")]
        [TestCaseSource(nameof(TransitiveData))]
        public bool InequalityOperator_Transitive(Post firstPost, Post secondPost, Post thirdPost)
        {
            bool xNotEqualToY = firstPost != secondPost;
            bool yNotEqualToZ = secondPost != thirdPost;
            bool xNotEqualToZ = firstPost != thirdPost;

            // Ensure repeat calls returns the same value
            Assert.AreEqual(xNotEqualToY, firstPost != secondPost);
            Assert.AreEqual(yNotEqualToZ, secondPost != thirdPost);
            Assert.AreEqual(xNotEqualToZ, firstPost != thirdPost);

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
        [Description("Tests usage of the inequality operator with NULL and a valid post object")]
        public void InequalityOperator_Null()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(DbOptions))
            {
                Post post = new Post(context, "Test Post", "<p>Post Content</p>", 56);

                // Ensure checking against NULL returns true
                Assert.Multiple(() =>
                {
                    Assert.IsTrue(post != null);
                    Assert.IsTrue(post != null);
                });

                // Ensure checking with a null post throws an exception
                Post nullPost = null;
                Assert.Throws<NullReferenceException>(() => nullPost.Equals(null));
            }
        }

        #endregion

        #region Helper Functions

        private int GenerateHashFromPost(Post post)
        {
            HashCode hashGenerator = new HashCode();
            hashGenerator.Add(post.Id);
            hashGenerator.Add(post.Title);
            hashGenerator.Add(post.Link);
            hashGenerator.Add(post.CreationTime);
            hashGenerator.Add(post.PublishTime);
            hashGenerator.Add(post.LastModificationTime);
            hashGenerator.Add(post.Category);
            hashGenerator.Add(post.Content);
            hashGenerator.Add(post.Description);

            return hashGenerator.ToHashCode();
        }

        #endregion
    }
}
