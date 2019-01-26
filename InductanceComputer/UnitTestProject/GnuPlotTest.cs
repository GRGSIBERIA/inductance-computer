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

        [TestMethod]
        public void TestPlot()
        {
            var gnu = new GnuPlot.GnuPlot("test.dat", "test.plt");
            const int N = 100;
            var x = new double[N];
            var y = new double[N];

            for (int i = 0; i < N; ++i)
            {
                x[i] = (double)i * 0.1;
                y[i] = 1.0 / (double)i;
            }

            gnu.FetchData(x, y);
            gnu.SetPlot("");
        }
    }
}
