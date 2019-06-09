using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratchet.IO.Format;

namespace UnitTest
{
    [TestClass]
    public class Basic
    {
        [TestMethod]
        public void String()
        {
            var result = JSON.Parse("\"unittest\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("unittest", ((result as JSON.String).Value));
        }

        [TestMethod]
        public void Number()
        {
            var result = JSON.Parse("123");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("123", ((result as JSON.Number).Value));
        }

        [TestMethod]
        public void Bool()
        {
            var result = JSON.Parse("true");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Boolean);
            Assert.AreEqual<bool>(true, ((result as JSON.Boolean).State));
        }

        [TestMethod]
        public void Array()
        {
            var result = JSON.Parse("[\"a\",1,true]");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Array);
            Assert.AreEqual<int>(3, ((result as JSON.Array).Values.Count));

            Assert.IsTrue(((result as JSON.Array).Values[0] is JSON.String));
            Assert.IsTrue(((result as JSON.Array).Values[1] is JSON.Number));
            Assert.IsTrue(((result as JSON.Array).Values[2] is JSON.Boolean));
        }


        [TestMethod]
        public void Object()
        {
            var result = JSON.Parse("{\"prop\":1}");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Object);
            Assert.IsTrue(((result as JSON.Object)["prop"]) is JSON.Number);
        }

        [TestMethod]
        public void Null()
        {
            var result = JSON.Parse("null");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Null);
        }
    }
}
