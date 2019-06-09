using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ratchet.IO.Format;


namespace UnitTest
{
    [TestClass]
    public class Number
    {
        static void AreEqualDouble(double expected, double actual, double epsilon = double.Epsilon)
        {
            Assert.IsTrue(expected <= actual + epsilon);
            Assert.IsTrue(expected >= actual - epsilon);
        }

        static void AreEqualFloat(float expected, float actual, float epsilon = float.Epsilon)
        {
            Assert.IsTrue(expected <= actual + epsilon);
            Assert.IsTrue(expected >= actual - epsilon);
        }

        [TestMethod]
        public void Zero()
        {
            var result = JSON.Parse("0");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("0", (result as JSON.Number).Value);
            Assert.AreEqual<long>(0, (long)(result as JSON.Number));
            Assert.AreEqual<int>(0, (int)(result as JSON.Number));
            AreEqualDouble(0, (double)(result as JSON.Number));
            AreEqualFloat(0f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void One()
        {
            var result = JSON.Parse("1");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("1", (result as JSON.Number).Value);
            Assert.AreEqual<long>(1, (long)(result as JSON.Number));
            Assert.AreEqual<int>(1, (int)(result as JSON.Number));
            AreEqualDouble(1, (double)(result as JSON.Number));
            AreEqualFloat(1f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void MinusOne()
        {
            var result = JSON.Parse("-1");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("-1", (result as JSON.Number).Value);
            Assert.AreEqual<long>(-1, (long)(result as JSON.Number));
            Assert.AreEqual<int>(-1, (int)(result as JSON.Number));
            AreEqualDouble(-1, (double)(result as JSON.Number));
            AreEqualFloat(-1f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void NegativeExp()
        {
            var result = JSON.Parse("1E-10");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("1E-10", (result as JSON.Number).Value);
            Assert.AreEqual<long>(0, (long)(result as JSON.Number));
            Assert.AreEqual<int>(0, (int)(result as JSON.Number));
            AreEqualDouble(1E-10, (double)(result as JSON.Number));
            AreEqualFloat(1E-10f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void PositiveExp()
        {
            var result = JSON.Parse("1E10");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("1E10", (result as JSON.Number).Value);
            Assert.AreEqual<long>(10000000000, (long)(result as JSON.Number));
            AreEqualDouble(1E10, (double)(result as JSON.Number));
            AreEqualFloat(1E10f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void DecimalToLongConv()
        {
            var result = JSON.Parse("50.1");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("50.1", (result as JSON.Number).Value);
            Assert.AreEqual<long>(50, (long)(result as JSON.Number));
            AreEqualDouble(50.1, (double)(result as JSON.Number));
            AreEqualFloat(50.1f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void NegativeDecimalToLongConv()
        {
            var result = JSON.Parse("-50.1");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("-50.1", (result as JSON.Number).Value);
            Assert.AreEqual<long>(-50, (long)(result as JSON.Number));
            AreEqualDouble(-50.1, (double)(result as JSON.Number));
            AreEqualFloat(-50.1f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void DecimalToLongConvWithExp()
        {
            var result = JSON.Parse("50.1234E2");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("50.1234E2", (result as JSON.Number).Value);
            Assert.AreEqual<long>(5012, (long)(result as JSON.Number));
            AreEqualDouble(50.1234E2, (double)(result as JSON.Number), 1E-12);
            AreEqualFloat(50.1234E2f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void NegativeDecimalToLongConvWithExp()
        {
            var result = JSON.Parse("-50.1234E2");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("-50.1234E2", (result as JSON.Number).Value);
            Assert.AreEqual<long>(-5012, (long)(result as JSON.Number));
            AreEqualDouble(-50.1234E2, (double)(result as JSON.Number), 1E-12);
            AreEqualFloat(-50.1234E2f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void DecimalToLongConvWithNegativeExp()
        {
            var result = JSON.Parse("50.1234E-1");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("50.1234E-1", (result as JSON.Number).Value);
            Assert.AreEqual<long>(5, (long)(result as JSON.Number));
            AreEqualDouble(50.1234E-1, (double)(result as JSON.Number));
            AreEqualFloat(50.1234E-1f, (float)(result as JSON.Number));
        }

        [TestMethod]
        public void NegativeDecimalToLongConvWithNegativeExp()
        {
            var result = JSON.Parse("-50.1234E-1");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JSON.Number);
            Assert.AreEqual<string>("-50.1234E-1", (result as JSON.Number).Value);
            Assert.AreEqual<long>(-5, (long)(result as JSON.Number));
            AreEqualDouble(-50.1234E-1, (double)(result as JSON.Number));
            AreEqualFloat(-50.1234E-1f, (float)(result as JSON.Number));
        }
    }
}
