using System;
using System.ComponentModel.DataAnnotations;

namespace CoderAndy.Models.Blog
{
    /// <summary>
    /// A Post within the blog
    /// </summary>
    public class Post
    {
        #region Properties

        /// <summary>
        /// Database ID of this post
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Title of the blog post
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// URL link of blog post in the format of: {domain}/blog/{parent-category-link/{category-link}/{post-link}
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Link { get; set; }

        /// <summary>
        /// Date and time this post was created
        /// </summary>
        public DateTime CreationTime { get; private set; }
        /// <summary>
        /// Date and time this post will become published
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// Date and time this post was last modified
        /// </summary>
        public DateTime LastModificationTime { get; private set; }

        /// <summary>
        /// Category this post is associated with
        /// </summary>
        [Required]
        public Category Category { get; set; }

        /// <summary>
        /// Content of this post
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Description of the post, displayed on links to this post
        /// </summary>
        [MaxLength(280)]
        public string Description { get; set; }

        /// <summary>
        /// Thumbnail for this post for links to this post
        /// </summary>
        //public Image Thumbnail { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Is this post currently published?
        /// </summary>
        /// <param name="a_currentTime">Current time</param>
        /// <returns></returns>
        public bool IsPublished(DateTime a_currentTime)
        {
            return a_currentTime >= PublishTime;
        }

        #endregion

        #region Constructors

        public Post()
            : this(null, null, DateTime.Today, Category.Uncategorised, null, null/*, null*/)
        {

        }

        public Post(string a_title, string a_content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="a_title">Title of the blog post</param>
        /// <param name="a_category">Category to associate this post with</param>
        /// <param name="a_content">Content of this post</param>
        public Post(string a_title, Category a_category, string a_content)
            : this(a_title, BlogHelper.NameToLinkName(a_title), DateTime.Today, a_category, a_content, null/*, null*/)
        {
            
        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="a_title">Title of the blog post</param>
        /// <param name="a_link">URL link of blog post in the format of: {domain}/blog/{category-link}/{post-link}</param>
        /// <param name="a_publishTime">Date and time this post should be published and available</param>
        /// <param name="a_category">Category to associate this post with</param>
        /// <param name="a_content">Content of this post</param>
        /// <param name="a_description">Description of the post, displayed on links to this post</param>
        /// <param name="a_thumbnail">Thumbnail for this post for links to this post</param>
        public Post(string a_title, string a_link, DateTime a_publishTime, Category a_category, string a_content, string a_description/*, Image a_thumbnail*/)
        {
            Title                   = a_title;
            Link                    = a_link;
            PublishTime             = a_publishTime;
            LastModificationTime    = DateTime.Today;
            CreationTime            = DateTime.Today;
            Category                = a_category;
            Content                 = a_content;
            Description             = a_description;
            //Thumbnail               = a_thumbnail;
        }

        #endregion
    }
}
