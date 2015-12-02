using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBTek.BugGuardian.Factories;

namespace BugGuardian.Tests.Factories
{
    [TestClass]
    public class ConfigurationFactoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationEmptyUrlTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration("", value, value, value, value, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationNullUrlTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration(null, value, value, value, value, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationEmptyUsernameTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration(value, "", value, value, value, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationNullUsernameTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration( value, null, value, value, value, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationEmptyPasswordTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration( value, value, "", value, value, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationNullPasswordTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration( value, value, null, value, value, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationEmptyProjectNameTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration(value, value,  value,  value, "", true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetConfigurationNullProjectNameTest()
        {
            var value = "fakeValue";
            ConfigurationFactory.SetConfiguration(value, value,  value,  value, null, true);
        }
    }
}
