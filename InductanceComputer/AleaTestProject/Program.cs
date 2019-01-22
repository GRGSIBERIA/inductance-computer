using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;

namespace AleaTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            AreaCore.RunParallel();
        }
    }

    public class AreaCore
    {
        public static double[] RunParallel()
        {
            double[] result = new double[100 * 100 * 100];

            Gpu.Default.For(0, result.Length, n =>
            {
                result[n] = (double)n;
            });

            return result;
        }
    }
}
