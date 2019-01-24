using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaComputing
{
    public class Field
    {
        public double Inductance { get; private set; }

        public Vector3 Position { get; private set; }
        public Vector3 Size { get; private set; }

        public Field(Vector3 position, double width, double height, double depth)
        {
            Size = new Vector3(width, height, depth);
            Position = position;
        }
    }
}
