using System.ComponentModel.DataAnnotations;

namespace CoderAndy.Models.Blog
{
    public class Category
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Category Parent { get; set; }

        #endregion

        #region Constructors

        public Category(string a_name, Category a_parent = null)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
