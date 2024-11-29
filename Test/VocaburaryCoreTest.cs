using Microsoft.VisualStudio.TestTools.UnitTesting;
using VocaburaryCore;

namespace Test
{
    [TestClass]
    public class VocaburaryCoreTest
    {
        [TestMethod]
        public void IntDateTest()
        {
            VocaburaryManagement testObject = new VocaburaryManagement();
            Assert.AreEqual(true, testObject.WhetherDateCorrect(20201212));
            Assert.AreEqual(false, testObject.WhetherDateCorrect(20201412));
            Assert.AreEqual(false, testObject.WhetherDateCorrect(20201242));
            Assert.AreEqual(false, testObject.WhetherDateCorrect(20206666));
            Assert.AreEqual(true, testObject.WhetherDateCorrect(20200229));
            Assert.AreEqual(false, testObject.WhetherDateCorrect(20220229));
        }
    }
}
