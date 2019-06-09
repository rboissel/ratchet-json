using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratchet.IO.Format;


namespace UnitTest
{
    [TestClass]
    public class String
    {
        [TestMethod]
        public void Empty()
        {
            var result = JSON.Parse("\"\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("", (result as JSON.String).Value);
        }

        [TestMethod]
        public void EscapeQuote()
        {
            var result = JSON.Parse("\"\\\"\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("\"", (result as JSON.String).Value);
        }

        [TestMethod]
        public void QuotedText()
        {
            var result = JSON.Parse("\"\\\"test\\\"\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("\"test\"", (result as JSON.String).Value);
        }

        [TestMethod]
        public void EscapeBackslash()
        {
            var result = JSON.Parse("\"\\\\test\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("\\test", (result as JSON.String).Value);
        }

        [TestMethod]
        public void EscapeTab()
        {
            var result = JSON.Parse("\"\\ttest\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("\ttest", (result as JSON.String).Value);
        }

        [TestMethod]
        public void EscapeNewLine()
        {
            var result = JSON.Parse("\"test\n\r\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("test\n\r", (result as JSON.String).Value);
        }

        [TestMethod]
        public void SpecialChar()
        {
            var result = JSON.Parse("\"Ŭŧƒ∞ is Magick ! █▌▐█ ☺☻♥♦♠♣\"");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.String);
            Assert.AreEqual<string>("Ŭŧƒ∞ is Magick ! █▌▐█ ☺☻♥♦♠♣", (result as JSON.String).Value);
        }
    }
}
