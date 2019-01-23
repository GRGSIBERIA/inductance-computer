using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.FSharp.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace CudaComputing
{
    public struct double3 : IEquatable<double3>, IStructuralEquatable, IComparable<double3>, IComparable, IStructuralComparable
    {
        public double x, y, z;

        public double3(double x, double y, double z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        #region Compare
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
        #endregion

        public static double3 operator +(double3 a, double3 b)
        {
            return new double3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static double3 operator -(double3 a, double3 b)
        {
            return new double3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static double3 operator /(double3 a, double b)
        {
            double c = 1.0 / b;
            return new double3(c * a.x, c * a.y, c * a.z);
        }

        public static double3 operator *(double3 a, double b)
        {
            return new double3(a.x * b, a.y * b, a.z * b);
        }

        public static double3 operator *(double b, double3 a)
        {
            return new double3(a.x * b, a.y * b, a.z * b);
        }

        public static double3 operator *(double3 a, double3 b)
        {
            return new double3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
                );
        }

        /// <summary>
        /// ベクトルの内積，内部で正規化はおこなわない　
        /// </summary>
        /// <param name="a">ベクトルA</param>
        /// <param name="b">ベクトルB</param>
        /// <returns>内積</returns>
        public static double Dot(double3 a, double3 b)
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
        public double3 Normalized
        {
            get
            {
                return this / this.Length;
            }
        }
    }

    [CompilationMapping(SourceConstructFlags.ObjectType)]
    [Struct]
    public struct Quaternion : IEquatable<Quaternion>, IStructuralEquatable, IComparable<Quaternion>, IComparable, IStructuralComparable
    {
        public double x, y, z, w;

        public double3 V
        {
            get
            {
                return new double3(x, y, z);
            }
        }

        [ReflectedDefinition]
        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        public Quaternion(double3 vec, double w)
        {
            x = vec.x; y = vec.y; z = vec.z; this.w = w;
        }

        #region Compare
        public int CompareTo(object other, IComparer comparer)
        {
            return (object)this == other ? 1 : 0;
        }

        public int CompareTo(Quaternion other)
        {
            return other.x == x && other.y == y && other.z == z && other.w == w ? 1 : 0;
        }

        public int CompareTo(object obj)
        {
            return (object)this == obj ? 1 : 0;
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            var obj = (Quaternion)other;
            return x == obj.x && y == obj.y && z == obj.z && w == obj.w;
        }

        public bool Equals(Quaternion other)
        {
            return other.x == x && other.y == y && other.z == z && other.w == w;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
        }
        #endregion

        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            var A = a.V;
            var B = b.V;
            var v = a.w * B + A * b.w + A * B;

            return new Quaternion(v, a.w * b.w - double3.Dot(A, B));
        }

        public static double3 operator *(Quaternion Q, double3 vec)
        {
            var R = Q.Conj.Normalized;
            var P = new Quaternion(vec, 0.0);
            var cos = Math.Cos(Q.w * 0.5);
            var sin = Math.Sin(Q.w * 0.5);

            var ret = R.Normalized * P * Q.Normalized;
            return new double3(ret.x, ret.y, ret.z);
        }

        public static Quaternion operator *(Quaternion a, double b)
        {
            return new Quaternion(a.x * b, a.y * b, a.z * b, a.w * b);
        }

        public static Quaternion operator *(double b, Quaternion a)
        {
            return new Quaternion(a.x * b, a.y * b, a.z * b, a.w * b);
        }

        /// <summary>
        /// 長さ
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(x * x + y * y + z * z + w * w);
            }
        }

        /// <summary>
        /// 正規化
        /// </summary>
        public Quaternion Normalized
        {
            get
            {
                var l = 1.0 / this.Length;
                return new Quaternion(x * l, y * l, z * l, w * l);
            }
        }

        /// <summary>
        /// 共役クォータニオン
        /// </summary>
        public Quaternion Conj
        {
            get
            {
                var Q = new Quaternion();
                Q.x = -x; Q.y = -y; Q.z = -z; Q.w = w;
                return Q;
            }
        }

        /// <summary>
        /// 逆数クォータニオン
        /// </summary>
        public Quaternion Inv
        {
            get
            {
                double len = this.Length;
                return (1.0 / (len * len)) * this.Conj;
            }
        }

        /// <summary>
        /// 軸と回転角からクォータニオンを生成する
        /// </summary>
        /// <param name="axis">軸</param>
        /// <param name="angle">回転角</param>
        /// <returns>軸周りのクォータニオン</returns>
        public static Quaternion AxisAngle(double3 axis, double angle)
        {
            return new Quaternion(axis.Normalized, angle);
        }
    }
}
