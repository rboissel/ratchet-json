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

        [TestMethod]
        public void Neasted()
        {
            var result = JSON.Parse("{\"level\":1, \"neasted\":{\"level\":2} }");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Object);
            Assert.AreEqual<string>("1", ((result as JSON.Object)["level"] as JSON.Number).Value);
            Assert.AreEqual<string>("2", (((result as JSON.Object)["neasted"] as JSON.Object)["level"] as JSON.Number).Value);
        }

        [TestMethod]
        public void BracesAsKey()
        {
            var result = JSON.Parse("{\"{}\":1}");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Object);
            Assert.AreEqual<string>("1", ((result as JSON.Object)["{}"] as JSON.Number).Value);
        }
    }
}
