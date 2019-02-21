using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoderAndy.Models.Blog
{
    public class Post
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime ModificationTime { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public PostContent Content { get; set; }

        public List<string> Tags { get; set; }

        #endregion

        #region Constructors

        public Post(string a_name, DateTime a_creationTime, Category a_category, string a_content, string[] a_tags)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
