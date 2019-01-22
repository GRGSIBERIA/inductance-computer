using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace CudaComputing
{
    public struct Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3(double x, double y, double z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        public Vector3(double[] v)
        {
            x = v[0]; y = v[1]; z = v[2];
        }

        public static Vector3 Zero = new Vector3(0.0, 0.0, 0.0);

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            double c = 1.0 / b;
            return new Vector3(c * a.x, c * a.y, c * a.z);
        }

        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator *(double b, Vector3 a)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
                );
        }

        public static double Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public Vector3 Normalized
        {
            get
            {
                return this / this.Length;
            }
        }
    }

    public struct Quaternion
    {
        public double x;
        public double y;
        public double z;
        public double w;


        public Quaternion(double x, double y, double z, double w)
        {
            v = new double[4];
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        public Quaternion(double[] v)
        {
            x = v[0]; y = v[1]; z = v[2]; w = v[3];
        }

        public Quaternion(Vector3 vec, double w)
        {
            x = vec.x; y = vec.y; z = vec.z; this.w = w;
        }

        public static Quaternion operator+(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x,
                a.w * b.w - (a.x * b.x + a.y * b.y + a.z * b.z));
        }

        public static Vector3 operator *(Quaternion Q, Vector3 vec)
        {
            var R = Q.Conj.Normalized;
            var P = new Quaternion(vec, 0.0);
            var cos = Math.Cos(Q.w * 0.5);
            var sin = Math.Sin(Q.w * 0.5);

            var ret = R * P * Q.Normalized;
            return new Vector3(ret.x, ret.y, ret.z);
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z + w * w);
            }
        }

        public Quaternion Normalized
        {
            get
            {
                var l = 1.0 / this.Length;
                return new Quaternion(x * l, y * l, z * l, w * l);
            }
        }

        public Quaternion Conj
        {
            get
            {
                return new Quaternion(-x, -y, -z, w);
            }
        }

        public static Quaternion AxisAngle(Vector3 axis, double angle)
        {
            return new Quaternion(axis.Normalized, angle);
        }
    }
}
