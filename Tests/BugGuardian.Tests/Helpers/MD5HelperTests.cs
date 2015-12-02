using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBTek.BugGuardian.Helpers;

namespace BugGuardian.Tests.Helpers
{
    [TestClass]
    public class MD5HelperTests
    {
        [TestMethod]
        public void MD5HashNullString()
            => Assert.AreEqual(string.Empty, MD5.GetMd5String(null));        

        [TestMethod]
        public void MD5HashEmptyString()
            => Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", MD5.GetMd5String(string.Empty));        

        [TestMethod]
        public void MD5HashString()
            => Assert.AreEqual("0fd3dbec9730101bff92acc820befc34", MD5.GetMd5String("Test string"));        

        [TestMethod]
        public void MD5HashStringComplex()
        {
            var complexString = "In C++ code we just declare strings as wchar_t (wide char?) instead of char and use the wcs functions instead of the str functions (for example wcscat and wcslen instead of strcat and strlen). To create a literal UCS-2 string in C code you just put an L before it as so:";
            var expected = "386b47ade851ac7df0aaa40e91e4d3e9";
            Assert.AreEqual(expected, MD5.GetMd5String(complexString));
        }
    }
}
