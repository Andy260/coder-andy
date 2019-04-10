using System;
using System.Text.RegularExpressions;

namespace CoderAndy.Models.Blog
{
    public static class BlogHelper
    {
        #region Public Functions

        /// <summary>
        /// Converts a name to a URL safe name
        /// </summary>
        /// <param name="name">Name to convert</param>
        /// <returns>URL Safe name</returns>
        public static string NameToLinkName(string name)
        {
            if (name == null)
            {
                return "_";
            }

            name = name.ToLower();

            // Ensure similar behaviour with input which is already a safe URL
            name = name.Replace("-", " ");

            // Remove unsafe URL characters
            name = new Regex("[^a-zA-Z0-9 ]").Replace(name, "");

            // Replace white-space with dashes, so output has the equivalent 
            // format: " Some Name " to "some-Name"
            string[] splitLink = name.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            name = string.Join("-", splitLink);

            // Ensure we don't return an empty string
            if (string.IsNullOrEmpty(name))
            {
                name = "_";
            }

            return name;
        }

        #endregion
    }
}
