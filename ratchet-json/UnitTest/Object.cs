using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratchet.IO.Format;


namespace UnitTest
{
    [TestClass]
    public class Object
    {
        [TestMethod]
        public void Empty()
        {
            var result = JSON.Parse("{}");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Object);
        }

        [TestMethod]
        public void EmptyNeasted()
        {
            var result = JSON.Parse("{\"neasted\":{}}");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Object);
            Assert.IsTrue(((result as JSON.Object)["neasted"]) is JSON.Object);
        }

        [TestMethod]
        public void NonExistantElement()
        {
            var result = JSON.Parse("{\"present\":true}");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Object);
            Assert.ThrowsException<KeyNotFoundException>(() => { object dummy = (result as JSON.Object)["notpresent"]; });
        }
    }
}
