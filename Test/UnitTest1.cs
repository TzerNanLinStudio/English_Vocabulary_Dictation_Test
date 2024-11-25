using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
