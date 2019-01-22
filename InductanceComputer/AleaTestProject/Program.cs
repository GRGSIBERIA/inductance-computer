using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Alea;
using Alea.Parallel;
using CudaComputing;
using System.Collections;

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

        static void RunVector()
        {
            var watch = new Stopwatch();
            watch.Start();
            var result = AreaCore.RunVector();
            watch.Stop();

            Console.WriteLine($"RunVector: {watch.ElapsedMilliseconds} ms");

            for (int i = 0; i < result.Length; ++i)
            {
                Debug.Assert((float)i == result[i].x);
            }
        }

        static void RunDouble3()
        {
            var watch = new Stopwatch();
            watch.Start();
            var result = AreaCore.RunDouble3();
            watch.Stop();

            Console.WriteLine($"RunDouble3: {watch.ElapsedMilliseconds} ms");

            for (int i = 0; i < result.Length; ++i)
            {
                Debug.Assert((double)i == result[i].x);
            }
        }

        static void Main(string[] args)
        {
            RunParallel();
            RunSquare();
            RunVector();
            RunDouble3();
        }
    }

    [GpuManaged]
    public class AreaCore
    {
        const int N = 100 * 100 * 100;

        public static double[] RunParallel()
        {
            double[] result = new double[N];

            Gpu.Default.For(0, result.Length, n =>
            {
                result[n] = (double)n;
            });

            return result;
        }

        public static double[] RunSquare()
        {
            double[] result = new double[N];

            Gpu.Default.For(0, result.Length, n =>
            {
                result[n] = (double)n * (double)n;
            });

            return result;
        }

        public static float3[] RunVector()
        {
            float3[] result = new float3[N];

            Gpu.Default.For(0, result.Length, n =>
            {
                var v = (float)n;
                result[n] = new float3(v, v, v);
            });

            return result;
        }

        public static double3[] RunDouble3()
        {
            double3[] result = new double3[N];

            Gpu.Default.For(0, result.Length, n =>
            {
                result[n] = new double3();
                result[n].x = (double)n;
            });

            return result;
        }
    }


    /*
    public struct double3 : IEquatable<double3>, IStructuralEquatable, IComparable<double3>, IComparable, IStructuralComparable
    {
        public double x, y, z;

        public int CompareTo(object other, IComparer comparer)
        {
            return (object)this == other ? 1 : 0;
        }

        public int CompareTo(double3 other)
        {
            return other.x == x && other.y == y && other.z == z ? 1 : 0;
        }

        public int CompareTo(object obj)
        {
            return (object)this == obj ? 1 : 0;
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            var obj = (double3)other;
            return x == obj.x && y == obj.y && z == obj.z;
        }

        public bool Equals(double3 other)
        {
            return other.x == x && other.y == y && other.z == z;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
    }
    */
}
