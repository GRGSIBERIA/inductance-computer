using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaComputing
{
    public class Coil
    {
        public double3 Position { get; private set; }
        public double3 Forward { get; private set; }
        public double3 Right { get; private set; }
        public float Radius { get; private set; }
        public float Height { get; private set; }

        public Coil(double3 position, double3 forward, double3 right, float radius, float height)
        {
            Position = position;
            Forward = forward;
            Right = right;
            Radius = radius;
            Height = height;
        }
    }

    public class CoilManager
    {
    }
}
