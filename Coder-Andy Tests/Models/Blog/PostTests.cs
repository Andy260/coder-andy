using System;
using CoderAndy.Models.Media;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace CoderAndy.Models.Blog.Tests
{
    [TestFixture]
    [Parallelizable]
    [TestOf(typeof(Post))]
    public class PostTests
    {
        #region Constructor Tests

        [Test]
        [Category("Constructor Test")]
        [Description("Tests expected usage of the full constructor")]
        public void FullConstructor()
        {
            string title            = "Test Post";
            string link             = "test-post";
            DateTime publishTime    = new DateTime(2019, 3, 1);
            Category category       = new Category("Test Category", null);
            string content          = "<p>Test Content</p>";
            string description      = "Test Post description";
            //Image thumbnail         = new Image(new Uri("/images/test-image.jpg", UriKind.Relative));

            Post post = new Post(title, link, publishTime, category, content, description/*, thumbnail*/);

            // Ensure created post object has expected values
            Assert.AreEqual(title, post.Title);
            Assert.AreEqual(link, post.Link);
            Assert.AreEqual(DateTime.Today, post.CreationTime);
            Assert.AreEqual(DateTime.Today, post.LastModificationTime);
            Assert.AreEqual(publishTime, post.PublishTime);
            Assert.AreEqual(category, post.Category);
            Assert.AreEqual(content, post.Content);
            Assert.AreEqual(description, post.Description);
            //Assert.AreEqual(thumbnail, post.Thumbnail);
        }

        [Test]
        [Category("Constructor Test")]
        [Description("Tests expected usage of the partial constructor")]
        public void PartialConstructor()
        {
            string title        = "Test Post";
            Category category   = new Category("Test Category", null);
            string content      = "<p>Test Content</p>";

            Post post = new Post(title, category, content);

            // Ensure created post object has expected values
            Assert.AreEqual(title, post.Title);
            Assert.AreEqual("test-post", post.Link);
            Assert.AreEqual(DateTime.Today, post.CreationTime);
            Assert.AreEqual(DateTime.Today, post.LastModificationTime);
            Assert.AreEqual(DateTime.Today, post.PublishTime);
            Assert.AreEqual(category, post.Category);
            Assert.AreEqual(content, post.Content);
            Assert.IsNull(post.Description);
            //Assert.IsNull(post.Thumbnail);
        }

        #endregion

        #region Public Function Tests

        [Test]
        [Category("Function Test")]
        [Description("Tests expected usage of IsPublished()")]
        public void IsPublished()
        {
            // Create Post object
            DateTime publishTime    = DateTime.UtcNow - TimeSpan.FromDays(1);
            Post post               = new Post("Another Test Post", "another-test-post", publishTime, null, null, null/*, null*/);

            // Ensure posts published in the past are considered published
            Assert.IsTrue(post.IsPublished(DateTime.UtcNow));

            // Ensure posts published in the future aren't considered published
            post.PublishTime += TimeSpan.FromDays(2);
            Assert.IsFalse(post.IsPublished(DateTime.UtcNow));
        }

        #endregion
    }
}
