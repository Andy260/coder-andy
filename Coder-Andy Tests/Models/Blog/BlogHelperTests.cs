using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CoderAndy.Models.Blog.Tests
{
    [TestFixture]
    [Parallelizable]
    [TestOf(typeof(BlogHelper))]
    public class BlogHelperTests
    {
        #region Test Case Sources

        public static class NameToLinkData
        {
            private static readonly char[] _validCharacters =
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };

            public static IEnumerable<string> InvalidCharacters
            {
                get
                {
                    List<string> invalidASCIIChars = new List<string>(char.MaxValue);
                    for (int i = char.MinValue; i < 128; i++)
                    {
                        char c = Convert.ToChar(i);
                        if (!char.IsControl(c) &&
                            !_validCharacters.Contains(c))
                        {
                            invalidASCIIChars.Add(c.ToString());
                        }
                    }

                    return invalidASCIIChars;
                }
            }
        }

        #endregion

        #region Function Tests

        [Test]
        [Category("Function Test")]
        [Description("Tests NameToLinkName() function with expected input")]
        [TestCase("Test Name", "test-name")]
        [TestCase("Test  Name", "test-name")]
        [TestCase(" Test Name ", "test-name")]
        public void NameToLinkName(string a_name, string a_expectedLink)
        {
            Assert.AreEqual(a_expectedLink, BlogHelper.NameToLinkName(a_name));
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests NameToLinkName() function with a single invalid character")]
        [TestCaseSource(typeof(NameToLinkData), "InvalidCharacters")]
        public void NameToLinkName_InvalidInputCharacter(string a_name)
        {
            Assert.AreEqual("_", BlogHelper.NameToLinkName(a_name));
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests NameToLinkName() function with the input \"Test {invalid-character} Name\"")]
        [TestCaseSource(typeof(NameToLinkData), "InvalidCharacters")]
        public void NameToLinkName_MixedValidAndInvalidInput(string a_invalidCharacter)
        {
            string name = string.Format("Test {0} Name", a_invalidCharacter);

            Assert.AreEqual("test-name", BlogHelper.NameToLinkName(name));
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests NameToLinkName() function with input which is already a valid link name")]
        public void NameToLinkName_LinkNameInput()
        {
            string linkName = "test-name";

            Assert.AreEqual(linkName, BlogHelper.NameToLinkName(linkName));
        }

        [Test]
        [Category("Function Test")]
        [Description("Tests NameToLinkName() function with a NULL string")]
        public void NameToLinkName_Null()
        {
            Assert.AreEqual("_", BlogHelper.NameToLinkName(null));
        }

        #endregion
    }
}
