using System;
using System.ComponentModel.DataAnnotations;

namespace CoderAndy.Models.Blog
{
    /// <summary>
    /// A Post within the blog
    /// </summary>
    public class Post : IEquatable<Post>
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

        #endregion
        
        #region Public Functions

        /// <summary>
        /// Is this post currently published?
        /// </summary>
        /// <param name="currentTime">Current time</param>
        /// <returns></returns>
        public bool IsPublished(DateTime currentTime)
        {
            return currentTime >= PublishTime;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Post);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(Post other)
        {
#pragma warning disable IDE0041 // Disable 'is null' check as this can't be simplified due to overriding of equality check behaviour

            // If parameter is null, then return false
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimisation for a common success case
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Return equality using all properties
            return Id == other.Id &&
                string.Equals(Title, other.Title, StringComparison.Ordinal) &&
                string.Equals(Link, other.Link, StringComparison.Ordinal) &&
                CreationTime == other.CreationTime &&
                PublishTime == other.PublishTime &&
                LastModificationTime == other.LastModificationTime &&
                Category == other.Category &&
                string.Equals(Content, other.Content, StringComparison.Ordinal) &&
                string.Equals(Description, other.Description, StringComparison.Ordinal);

#pragma warning restore IDE0041 // Re-enable 'is null' check for remaining code
        }

        /// <summary>
        /// Returns a hash code representing this object
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            HashCode hashGenerator = new HashCode();
            hashGenerator.Add(Id);
            hashGenerator.Add(Title);
            hashGenerator.Add(Link);
            hashGenerator.Add(CreationTime);
            hashGenerator.Add(PublishTime);
            hashGenerator.Add(LastModificationTime);
            hashGenerator.Add(Category);
            hashGenerator.Add(Content);
            hashGenerator.Add(Description);

            return hashGenerator.ToHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The title of the post</returns>
        public override string ToString()
        {
            return Title;
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Returns true if both operands are equal, otherwise false.
        /// </summary>
        /// <param name="lhs">Left hand side object</param>
        /// <param name="rhs">Right hand side object</param>
        /// <returns>Equality of operands</returns>
        public static bool operator ==(Post lhs, Post rhs)
        {
#pragma warning disable IDE0041 // Disable 'is null' check as this can't be simplified due to overriding of equality check behaviour

            // Check for null on left hand side
            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    // Both left and right hand side of operands are null
                    // so are both equal
                    return true;
                }

                // Only the left side is null, not equal
                return false;
            }

            // Equals handles the case of null on the right side
            // and object comparison
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Returns true if both operands are not equal, otherwise false.
        /// </summary>
        /// <param name="lhs">Left hand side object</param>
        /// <param name="rhs">Right hand side object</param>
        /// <returns>In-equality of operands</returns>
        public static bool operator !=(Post lhs, Post rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="id">Database ID of this post</param>
        public Post(int id = 0)
            : this(null, null, id)
        {

        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="title">Title of the blog post</param>
        /// <param name="content">HTML encoded content of this post</param>
        /// <param name="id">Database ID of this post</param>
        public Post(string title, string content, int id = 0)
            : this(title, Category.Uncategorised, content, id)
        {

        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="title">Title of the blog post</param>
        /// <param name="category">Category to associate this post with</param>
        /// <param name="content">HTML encoded content of this post</param>
        /// <param name="id">Database ID of this post</param>
        public Post(string title, Category category, string content, int id = 0)
            : this(title, BlogHelper.NameToLinkName(title), DateTime.Today, category, content, null, id)
        {

        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="title">Title of the blog post</param>
        /// <param name="link">URL link of blog post in the format of: {domain}/blog/{category-link}/{post-link}</param>
        /// <param name="publishTime">Date and time this post should be published and available</param>
        /// <param name="category">Category to associate this post with</param>
        /// <param name="content">HTML encoded content of this post</param>
        /// <param name="description">Description of the post, displayed on links to this post</param>
        /// <param name="thumbnail">Thumbnail for this post for links to this post</param>
        /// /// <param name="id">Database ID of this post</param>
        public Post(string title, string link, DateTime publishTime, Category category, string content, string description, int id = 0)
        {
            Id                      = id;
            Title                   = title;
            Link                    = link;
            PublishTime             = publishTime;
            LastModificationTime    = DateTime.Today;
            CreationTime            = DateTime.Today;
            Category                = category;
            Content                 = content;
            Description             = description;
        }

        #endregion
    }
}
