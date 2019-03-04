using System.ComponentModel.DataAnnotations;

namespace CoderAndy.Models.Blog
{
    /// <summary>
    /// Post category within the blog
    /// </summary>
    public class Category
    {
        #region Properties

        /// <summary>
        /// Database ID of this category
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Name of the blog category
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// URL link of blog category in the format of: {domain}/blog/{parent-category-link/{category-link}/{post-link}
        /// </summary>
        [Required]
        public string LinkName { get; set; }

        /// <summary>
        /// Parent category for this category (NULL if top-level category)
        /// </summary>
        public Category Parent { get; set; }

        #endregion

        #region Static Properties

        /// <summary>
        /// Default category for posts without a category set.
        /// </summary>
        public static readonly Category Uncategorised = new Category("Uncategorised", null);

        #endregion

        #region Constructors

        public Category()
            : this(null, null, null)
        {

        }

        /// <summary>
        /// Creates a blog category.
        /// </summary>
        /// <param name="a_name">Name of the blog category</param>
        /// <param name="a_parent">Parent category for this category (NULL if top-level category)</param>
        public Category(string a_name, Category a_parent)
            : this(a_name, a_name, a_parent)
        {
            
        }

        /// <summary>
        /// Creates a blog category.
        /// </summary>
        /// <param name="a_name">Name of the blog category</param>
        /// <param name="a_linkName">URL link of blog category in the format of: {domain}/blog/{parent-category-link/{category-link}/{post-link}</param>
        /// <param name="a_parent">Parent category for this category (NULL if top-level category)</param>
        public Category(string a_name, string a_linkName, Category a_parent)
        {
            Name        = a_name;
            LinkName    = BlogHelper.NameToLinkName(a_linkName);
            Parent      = a_parent;
        }

        #endregion
    }
}
