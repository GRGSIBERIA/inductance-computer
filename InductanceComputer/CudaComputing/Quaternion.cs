using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaComputing
{
    public struct Quaternion : IEquatable<Quaternion>, IStructuralEquatable, IComparable<Quaternion>, IComparable, IStructuralComparable
    {
        public double x, y, z, w;

        public Vector3 V
        {
            get
            {
                return Vector3.Create(x, y, z);
            }
        }

        public static Quaternion Create(double x, double y, double z, double w)
        {
            Quaternion Q;
            Q.x = x; Q.y = y; Q.z = z; Q.w = w;
            return Q;
        }

        public static Quaternion Create(Vector3 vec, double w)
        {
            Quaternion Q;
            Q.x = vec.x; Q.y = vec.y; Q.z = vec.z; Q.w = w;
            return Q;
        }

        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        public Quaternion(Vector3 vec, double w)
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
            return Create(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return Create(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            var A = a.V;
            var B = b.V;
            var v = a.w * B + A * b.w + A * B;

            return Create(v, a.w * b.w - Vector3.Dot(A, B));
        }

        public static Vector3 operator *(Quaternion Q, Vector3 vec)
        {
            var R = Q.Conj.Normalized;
            var P = Create(vec, 0.0);
            var cos = Math.Cos(Q.w * 0.5);
            var sin = Math.Sin(Q.w * 0.5);

            var ret = R.Normalized * P * Q.Normalized;
            return Vector3.Create(ret.x, ret.y, ret.z);
        }

        public static Quaternion operator *(Quaternion a, double b)
        {
            return Create(a.x * b, a.y * b, a.z * b, a.w * b);
        }

        public static Quaternion operator *(double b, Quaternion a)
        {
            return Create(a.x * b, a.y * b, a.z * b, a.w * b);
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
                return Create(x * l, y * l, z * l, w * l);
            }
        }

        /// <summary>
        /// 共役クォータニオン
        /// </summary>
        public Quaternion Conj
        {
            get
            {
                return Create(-x, -y, -z, w);
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
        public static Quaternion AxisAngle(Vector3 axis, double angle)
        {
            return Create(axis.Normalized, angle);
        }
    }
}
