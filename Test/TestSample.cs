using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TestSample
    {
        // Test for Add method (int)
        [TestMethod]
        public void TestAddMethod_Int()
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

        // Test for And method (bool)
        [TestMethod]
        public void TestAndMethod_Bool()
        {
            // Arrange
            var expected = true;
            var a = true;
            var b = true;

            // Act
            var result = And(a, b);

            // Assert
            Assert.AreEqual(expected, result);
        }

        public bool And(bool a, bool b)
        {
            return a && b;
        }

        // Test for Concat method (char)
        [TestMethod]
        public void TestConcatMethod_Char()
        {
            // Arrange
            var expected = "ab";
            var a = 'a';
            var b = 'b';

            // Act
            var result = Concat(a, b);

            // Assert
            Assert.AreEqual(expected, result);
        }

        public string Concat(char a, char b)
        {
            return a.ToString() + b.ToString();
        }

        // Test for Combine method (string)
        [TestMethod]
        public void TestCombineMethod_String()
        {
            // Arrange
            var expected = "hello world";
            var a = "hello";
            var b = " world";

            // Act
            var result = Combine(a, b);

            // Assert
            Assert.AreEqual(expected, result);
        }

        public string Combine(string a, string b)
        {
            return a + b;
        }

        // Test for Add method (double)
        [TestMethod]
        public void TestAddMethod_Double()
        {
            // Arrange
            var expected = 5.5;
            var a = 2.2;
            var b = 3.3;

            // Act
            var result = Add(a, b);

            // Assert
            Assert.AreEqual(expected, result, 0.0001); // Delta for floating-point precision
        }

        public double Add(double a, double b)
        {
            return a + b;
        }
    }
}
