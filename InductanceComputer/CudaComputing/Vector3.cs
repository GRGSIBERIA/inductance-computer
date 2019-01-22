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
        public float x;

        [FieldOffset(4)]
        public float y;

        [FieldOffset(8)]
        public float z;

        [FieldOffset(0)]
        public float[] v;

        public Vector3(float x, float y, float z)
        {
            v = new float[3];
            this.x = x; this.y = y; this.z = z;
        }

        public Vector3(float[] v)
        {
            this.v = new float[3];
            x = v[0]; y = v[1]; z = v[2];
        }

        public Vector3 operator+(Vector3 a)
        {
            
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Quaternion
    {
        [FieldOffset(0)]
        public float x;

        [FieldOffset(4)]
        public float y;

        [FieldOffset(8)]
        public float z;

        [FieldOffset(12)]
        public float w;

        [FieldOffset(0)]
        public float[] v;

        public Quaternion(float x, float y, float z, float w)
        {
            v = new float[4];
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        public Quaternion(float[] v)
        {
            this.v = new float[4];
            x = v[0]; y = v[1]; z = v[2]; w = v[3];
        }
    }
}
