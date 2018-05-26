using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
    [TestClass]
    public class ExampleTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(false, "should fail asdf");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.IsTrue(true, "works");
        }
    }
}
