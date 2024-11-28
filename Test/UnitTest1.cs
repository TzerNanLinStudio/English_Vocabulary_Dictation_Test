using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VocaburaryCore;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestAddMethod()
        {
            // Arrange
            var expected = 5;
            var a = 2;
            var b = 3;

            // Act
            var result = Add(a, b);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestMethod2()
        {
            int a = 5;
            int b = 5;

            // Assert
            Assert.AreEqual(a, b);
        }

        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
