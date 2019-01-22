using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;

namespace CudaComputing
{
    [GpuManaged]
    public class Wire
    {
        public Vector3 Position { get; private set; }
        public float FluxDensity { get; private set; }

        public Wire(Vector3 position, Coil[] coils)
        {
            Position = position;
            FluxDensity = 0f;
        }

        private void ComputeFluxDensity(Coil[] coils)
        {

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
