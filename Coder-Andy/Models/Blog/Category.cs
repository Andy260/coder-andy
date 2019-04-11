using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using CoderAndy.Data;
using Microsoft.EntityFrameworkCore;

namespace CoderAndy.Models.Blog
{
    /// <summary>
    /// Post category within the blog
    /// </summary>
    public class Category : IEquatable<Category>
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
        public string PermaLink { get; set; }

        /// <summary>
        /// Parent category for this category (NULL if top-level category)
        /// </summary>
        public Category Parent { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Default category for posts without a category set.
        /// </summary>
        /// <param name="dbContext">DbContext to retrieve the 'Uncategorised' category</param>
        /// <returns>Default category for posts without a category set.</returns>
        public static Category Uncategorised(ApplicationDbContext dbContext)
        {
            IQueryable<Category> uncategorised = from category in dbContext.Categories
                                                 where string.Equals("uncategorised", category.PermaLink)
                                                 select category;

            return uncategorised.First();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Category);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(Category other)
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
                string.Equals(Name, other.Name, StringComparison.Ordinal) &&
                string.Equals(PermaLink, other.PermaLink, StringComparison.Ordinal) &&
                Parent == other.Parent;

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
            hashGenerator.Add(Name);
            hashGenerator.Add(PermaLink);
            hashGenerator.Add(Parent);

            return hashGenerator.ToHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>The title of the category</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Creates the schema required for the blog Category model
        /// </summary>
        /// <param name="builder">The builder being used to construct the model</param>
        public static void BuildModel(ModelBuilder builder)
        {
            // Set alternate key
            builder.Entity<Category>().HasAlternateKey(c => c.PermaLink);

            // Set data with uncategorised category
            Category uncategorised = new Category("Uncategorised", "uncategorised", null, 1);
            builder.Entity<Category>().HasData(uncategorised);
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Returns true if both operands are equal, otherwise false.
        /// </summary>
        /// <param name="lhs">Left hand side object</param>
        /// <param name="rhs">Right hand side object</param>
        /// <returns>Equality of operands</returns>
        public static bool operator ==(Category lhs, Category rhs)
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

#pragma warning restore IDE0041 // Re-enable 'is null' check
        }

        /// <summary>
        /// Returns true if both operands are not equal, otherwise false.
        /// </summary>
        /// <param name="lhs">Left hand side object</param>
        /// <param name="rhs">Right hand side object</param>
        /// <returns>In-equality of operands</returns>
        public static bool operator !=(Category lhs, Category rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a blog category.
        /// </summary>
        /// <param name="name">Name of the blog category</param>
        /// <param name="linkName">URL link of blog category in the format of: {domain}/blog/{parent-category-link/{category-link}/{post-link}</param>
        /// <param name="parent">Parent category for this category (NULL if top-level category)</param>
        /// <param name="id">Database ID of this category</param>
        public Category(string name, string linkName, Category parent, int id = 0)
        {
            Id          = id;
            Name        = name;
            PermaLink    = BlogHelper.NameToLinkName(linkName);
            Parent      = parent;
        }

        /// <summary>
        /// Constructs a new Category
        /// </summary>
        /// <param name="id">Database ID of this category</param>
        public Category(int id = 0)
            : this(null, null, id)
        {

        }

        /// <summary>
        /// Creates a blog category.
        /// </summary>
        /// <param name="name">Name of the blog category</param>
        /// <param name="parent">Parent category for this category (NULL if top-level category)</param>
        /// <param name="id">Database ID of this category</param>
        public Category(string name, Category parent, int id = 0)
            : this(name, name, parent, id)
        {
            
        }

        #endregion
    }
}
