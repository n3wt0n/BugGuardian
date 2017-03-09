using DBTek.BugGuardian;
using DBTek.BugGuardian.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BugGuardian.Tests
{
    [TestClass]
    public class DirectCallTests
    {
        public DirectCallTests()
        {
            DBTek.BugGuardian.Factories.ConfigurationFactory.SetConfiguration("https://MY_URL:8080/TFS", "USERNAME", "PASSWORD", "PROJECT_NAME", false);
        }

        [TestMethod]
        public void DirectCallBugTest()
        {
            BugGuardianResponse result;
            using (var manager = new BugGuardianManager())
            {
                result = manager.AddBug(GenerateException(), message: "Exception from Unit Test", tags: new List<string> { "DirectCall" });
            }

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void DirectCallTaskTest()
        {
            BugGuardianResponse result;
            using (var manager = new BugGuardianManager())
            {
                result = manager.AddTask(GenerateException(), message: "Exception from Unit Test", tags: new List<string> { "DirectCall" });
            }

            Assert.IsTrue(result.Success);
        }

        private Exception GenerateException()
            => new Exception($"AutoException: {DateTime.Now}");
    }
}
