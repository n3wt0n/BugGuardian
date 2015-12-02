using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBTek.BugGuardian.Helpers;
using System.Collections.Generic;

namespace BugGuardian.Tests.Helpers
{
    [TestClass]
    public class ExceptionsHelperTests
    {

        [TestMethod]
        public void BuildExceptionStringTest()
        {
            var ex = new ArgumentNullException("", "fakeMessage") { Source = "fakeSource" };
            var expected = "<strong>fakeMessage</strong><br /><br />";

            Assert.AreEqual(expected, ExceptionsHelper.BuildExceptionString(ex));
        }

        [TestMethod]
        public void BuildExceptionStringUnhandledExceptionTest()
        {
            var innerEx = new NotImplementedException("fakeMessage") { Source = "fakeInnerSource" };
            var ex = new System.Web.HttpUnhandledException("message", innerEx) { Source = "fakeSource" };

            var expected = "<strong>fakeMessage</strong><br /><br />";
            Assert.AreEqual(expected, ExceptionsHelper.BuildExceptionString(ex));
        }

        [TestMethod]
        public void BuildExceptionStringInnerExceptionTest()
        {
            var innerEx = new NotImplementedException("fakeInnerMessage");
            var ex = new ArgumentNullException("fakeMessage", innerEx) { Source = "fakeSource" };

            var expected = "<strong>fakeMessage</strong><br /><br /><br /><br /><i><u>Inner Exception:</u></i><br /><strong>fakeInnerMessage</strong><br /><br />";
            Assert.AreEqual(expected, ExceptionsHelper.BuildExceptionString(ex));
        }

        [TestMethod]
        public void BuildExceptionStringMessageTest()
        {
            var ex = new ArgumentNullException("", "fakeMessage") { Source = "fakeSource" };
            var expected = "<strong>myMessage</strong><br /><br /><strong>fakeMessage</strong><br /><br />";

            Assert.AreEqual(expected, ExceptionsHelper.BuildExceptionString(ex, "myMessage"));
        }

        [TestMethod]        
        public void BuildExceptionStringNullExceptionTest()
            => Assert.AreEqual(string.Empty, ExceptionsHelper.BuildExceptionString(null));

        [TestMethod]
        public void BuildExceptionTitleTest()
        {
            var ex = new ArgumentNullException() {Source = "fakeSource" };
            var expected = "System.ArgumentNullException - fakeSource";
            Assert.AreEqual(expected, ExceptionsHelper.BuildExceptionTitle(ex));
        }

        [TestMethod]
        public void BuildExceptionTitleInnerExceptionTest()
        {
            var innerEx = new NotImplementedException();
            var ex = new ArgumentNullException("message", innerEx) { Source = "fakeSource" };
            
            var expected = "System.ArgumentNullException - fakeSource";
            Assert.AreEqual(expected, ExceptionsHelper.BuildExceptionTitle(ex));
        }

        [TestMethod]
        public void BuildExceptionTitleUnhandledExceptionTest()
        {
            var innerEx = new NotImplementedException() { Source = "fakeInnerSource" };
            var ex = new System.Web.HttpUnhandledException("message", innerEx) { Source = "fakeSource" };

            var expected = "System.NotImplementedException - fakeInnerSource";
            Assert.AreEqual(expected, ExceptionsHelper.BuildExceptionTitle(ex));
        }

        [TestMethod]
        public void BuildExceptionTitleNullExceptionTest()
            => Assert.AreEqual(string.Empty, ExceptionsHelper.BuildExceptionTitle(null));

        [TestMethod]
        public void BuildExceptionHashTest()
        {
            //TODO
        }

        [TestMethod]
        public void BuildExceptionHashUnhandledExceptionTest()
        {
            //TODO
        }

        [TestMethod]
        public void BuildExceptionHashInnerExceptionTest()
        {
            //TODO
        }

        [TestMethod]
        public void BuildExceptionHashNullExceptionTest()
            => Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", ExceptionsHelper.BuildExceptionHash(null));


    }
}
