using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class AleaTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            AleaTestProject.AreaCore.RunParallel();
        }
    }
}
