using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBTek.BugGuardian.Converters;

namespace BugGuardian.Tests.Converters
{
    [TestClass]
    public class StringToAsciiConverterTest
    {
        [TestMethod]
        public void StringToAsciiTest()
        {
            var plainText = "test to convert";            
            var expected = new byte[] { 116, 101, 115, 116, 32, 116, 111, 32, 99, 111, 110, 118, 101, 114, 116 };

            CollectionAssert.AreEqual(expected, StringToAsciiConverter.StringToAscii(plainText));            
        }

        [TestMethod]
        public void StringToAsciiEmptyTest()
            => CollectionAssert.AreEqual(new byte[] { }, StringToAsciiConverter.StringToAscii(string.Empty));        

        [TestMethod]
        public void StringToAsciiNullTest()
            => CollectionAssert.AreEqual(null, StringToAsciiConverter.StringToAscii(null));       
    }
}
