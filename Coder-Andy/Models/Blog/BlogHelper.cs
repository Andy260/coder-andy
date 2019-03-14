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
        /// <param name="a_name">Name to convert</param>
        /// <returns>URL Safe name</returns>
        public static string NameToLinkName(string a_name)
        {
            if (a_name == null)
            {
                return "_";
            }

            a_name = a_name.ToLower();

            // Ensure similar behaviour with input which is already a safe URL
            a_name = a_name.Replace("-", " ");

            // Remove unsafe URL characters
            a_name = new Regex("[^a-zA-Z0-9 ]").Replace(a_name, "");

            // Replace white-space with dashes, so output has the equivalent 
            // format: " Some Name " to "some-Name"
            string[] splitLink = a_name.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            a_name = string.Join("-", splitLink);

            // Ensure we don't return an empty string
            if (string.IsNullOrEmpty(a_name))
            {
                a_name = "_";
            }

            return a_name;
        }

        #endregion
    }
}
