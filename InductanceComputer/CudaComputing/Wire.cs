using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;

namespace CudaComputing
{
    public class Wire
    {
        public Vector3 Position { get; private set; }
        public double FluxDensity { get; private set; }

        public Wire(Vector3 position, Coil[] coils)
        {
            Position = position;
            FluxDensity = 0.0;
        }

        [GpuManaged]
        private void ComputeFluxDensity(Coil[] coils)
        {
            double[] result = new double[coils.Length];
            Gpu.Default.For(0, coils.Length, i =>
            {
                var v = coils[i].Position + coils[i].Forward;
                result[i] = v.Length;
            });

            FluxDensity = 0.0;
            foreach (var f in result)
                FluxDensity += f;
        }
    }
    
    public class WireManager
    {
        public Wire[] wires;

        public WireManager()
        {
            
        }
    }
}
