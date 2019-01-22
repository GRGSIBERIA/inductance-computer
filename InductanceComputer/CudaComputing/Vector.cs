﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace CudaComputing
{
    public struct Vector3
    {
        public double x, y, z;

        public Vector3(double x, double y, double z)
        {
            this.x = x; this.y = y; this.z = z;
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

    public struct Quaternion
    {
        public double x, y, z, w;

        public Vector3 V
        {
            get
            {
                return new Vector3(x, y, z);
            }
        }

        public static Quaternion Zero = new Quaternion(0.0, 0.0, 0.0, 0.0);

        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        public Quaternion(Vector3 vec, double w)
        {
            x = vec.x; y = vec.y; z = vec.z; this.w = w;
        }

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

            return new Quaternion(v, a.w * b.w - Vector3.Dot(A, B));
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
                return new Quaternion(-x, -y, -z, w);
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
            return new Quaternion(axis.Normalized, angle);
        }
    }
}
