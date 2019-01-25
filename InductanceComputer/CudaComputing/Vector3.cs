using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Alea;

namespace CudaComputing
{
    public struct Vector3 : IEquatable<Vector3>, IStructuralEquatable, IComparable<Vector3>, IComparable, IStructuralComparable
    {
        public double x, y, z;

        public static Vector3 Create(double x, double y, double z)
        {
            Vector3 v;
            v.x = x; v.y = y; v.z = z;
            return v;
        }

        public static Vector3 Create(int x, int y, int z)
        {
            Vector3 v;
            v.x = x; v.y = y; v.z = z;
            return v;
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        #region Compare
        public int CompareTo(object other, IComparer comparer)
        {
            return (object)this == other ? 1 : 0;
        }

        public int CompareTo(Vector3 other)
        {
            return other.x == x && other.y == y && other.z == z ? 1 : 0;
        }

        public int CompareTo(object obj)
        {
            return (object)this == obj ? 1 : 0;
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            var obj = (Vector3)other;
            return x == obj.x && y == obj.y && z == obj.z;
        }

        public bool Equals(Vector3 other)
        {
            return other.x == x && other.y == y && other.z == z;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        #endregion

        #region Operator
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return Create(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return Create(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            double c = 1.0 / b;
            return Create(c * a.x, c * a.y, c * a.z);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return Create(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3 operator /(Vector3 a, int3 b)
        {
            return Create(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3 operator *(Vector3 a, double b)
        {
            return Create(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator *(double b, Vector3 a)
        {
            return Create(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return Create(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
                );
        }
        #endregion

        /// <summary>
        /// ベクトルの内積，内部で正規化はおこなわない　
        /// </summary>
        /// <param name="a">ベクトルA</param>
        /// <param name="b">ベクトルB</param>
        /// <returns>内積</returns>
        public static double Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        /// <summary>
        /// ベクトルの長さ
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z);
            }
        }

        /// <summary>
        /// ベクトルの正規化
        /// </summary>
        public Vector3 Normalized
        {
            get
            {
                return this / this.Length;
            }
        }
    }
}
