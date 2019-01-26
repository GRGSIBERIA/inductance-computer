using Microsoft.VisualStudio.TestTools.UnitTesting;
using GnuPlot;
using System.Diagnostics;

namespace UnitTestProject
{
    [TestClass]
    public class GnuPlotTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var gnu = new GnuPlot.GnuPlot("test.dat", "test.plt");
            gnu.Execute();
        }
    }
}
