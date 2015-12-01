using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBTek.BugGuardian.Extensions;

namespace BugGuardian.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void NormalizeForHtmlTest()
        {
            var testString = $"first{Environment.NewLine}second";
            var expected = "first<br />second";

            Assert.AreEqual(expected, testString.NormalizeForHtml());
        }

        [TestMethod]
        public void NormalizeForHtmlLongTest()
        {
            var testString = $"first{Environment.NewLine}second.. third bla bla? \\ {Environment.NewLine} are you sure?";
            var expected = "first<br />second.. third bla bla? \\ <br /> are you sure?";

            Assert.AreEqual(expected, testString.NormalizeForHtml());
        }

        [TestMethod]
        public void NormalizeForHtmlEmptyTest()
            => Assert.AreEqual(string.Empty, string.Empty.NormalizeForHtml());        

        [TestMethod]
        public void NormalizeForHtmlNullTest()
        {
            string testString = null;

            Assert.AreEqual(string.Empty, testString.NormalizeForHtml());
        }
    }
}
