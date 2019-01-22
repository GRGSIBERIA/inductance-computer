using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using Alea;
using Alea.Parallel;


namespace AleaTestProject
{
    class Program
    {
        static void RunParallel()
        {
            var watch = new Stopwatch();
            watch.Start();
            var result = AreaCore.RunParallel();
            watch.Stop();

            Console.WriteLine($"RunParallel: {watch.ElapsedMilliseconds} ms");

            for (int i = 0; i < result.Length; ++i)
            {
                Debug.Assert((double)i == result[i]);
            }
        }

        static void RunSquare()
        {
            var watch = new Stopwatch();
            watch.Start();
            var result = AreaCore.RunSquare();
            watch.Stop();

            Console.WriteLine($"RunSquare: {watch.ElapsedMilliseconds} ms");

            for (int i = 0; i < result.Length; ++i)
            {
                Debug.Assert((double)i * (double)i == result[i]);
            }
        }

        static void Main(string[] args)
        {
            RunParallel();
            RunSquare();
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

        public static double[] RunSquare()
        {
            double[] result = new double[100 * 100 * 100];

            Gpu.Default.For(0, result.Length, n =>
            {
                result[n] = (double)n * (double)n;
            });

            return result;
        }
    }
}
