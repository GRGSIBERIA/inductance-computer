using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace CudaComputing
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector3
    {
        [FieldOffset(0)]
        public double x;

        [FieldOffset(8)]
        public double y;

        [FieldOffset(16)]
        public double z;

        [FieldOffset(24)]
        public double[] v;

        public Vector3(double x, double y, double z)
        {
            v = new double[3];
            this.x = x; this.y = y; this.z = z;
        }

        public Vector3(double[] v)
        {
            this.v = new double[3];
            x = v[0]; y = v[1]; z = v[2];
        }

        public static Vector3 operator+(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator-(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator/(Vector3 a, double b)
        {
            double c = 1.0 / b;
            return new Vector3(c * a.x, c * a.y, c * a.z);
        }

        public static Vector3 operator*(Vector3 a, double b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator *(double b, Vector3 a)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static double Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
                );
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Quaternion
    {
        [FieldOffset(0)]
        public double x;

        [FieldOffset(4)]
        public double y;

        [FieldOffset(8)]
        public double z;

        [FieldOffset(12)]
        public double w;

        [FieldOffset(0)]
        public double[] v;

        public Quaternion(double x, double y, double z, double w)
        {
            v = new double[4];
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        public Quaternion(double[] v)
        {
            this.v = new double[4];
            x = v[0]; y = v[1]; z = v[2]; w = v[3];
        }
    }
}
