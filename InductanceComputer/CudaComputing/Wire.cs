using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;

namespace CudaComputing
{
    public class Wire : IEquatable<Wire>, IStructuralEquatable, IComparable<Wire>, IComparable, IStructuralComparable
    {
        public Vector3 Position { get; private set; }
        public double FluxDensity { get; private set; }

        public Wire(Vector3 position)
        {
            Position = position;
        }

        [GpuManaged]
        public double ComputeFluxDensity(CoilManager coilManager)
        {
            var result = new double[coilManager.Coils.Length, coilManager.DivideTheta, coilManager.DivideRadius];

            var divideTheta = coilManager.DivideTheta;
            var divideRadius = coilManager.DivideRadius;
            var wirePosition = Position;            // 内部的に渡せないらしいので仮変数を作る

            for (int coilId = 0; coilId < coilManager.Coils.Length; ++coilId)
            {
                var coil = coilManager.Coils[coilId];   // コイルごとに穴埋めする

                Gpu.Default.For(0, coilManager.DivideRadius * coilManager.DivideTheta, id =>
                {
                    int rid = id % divideTheta;
                    int tid = id / divideTheta;

                    double dR = coil.Radius / (double)divideRadius * rid;
                    double dT = 2.0 * Math.PI / (double)divideTheta * tid;

                    // 共通変数の計算
                    double fracUp = coil.Sigma * dR;
                    var rot = Quaternion.Create(coil.Forward, dR) * coil.Right * dR;
                    var pos = rot - wirePosition - coil.Position;
                    var translate = coil.Forward * coil.Height * 0.5;

                    // コイルのプラス極の計算
                    double fracDownIn = (pos + translate).Length;
                    var fracDown = fracDownIn * fracDownIn * fracDownIn;
                    result[coilId, tid, rid] = fracUp / fracDown;

                    // コイルのマイナス極の磁束密度を引く
                    fracDownIn = (pos - translate).Length;
                    fracDown = fracDownIn * fracDownIn * fracDownIn;
                    result[coilId, tid, rid] = result[coilId, tid, rid] - fracUp / fracDown;
                });
            }

            Gpu.Default.Synchronize();  // 同期を取る

            FluxDensity = Gpu.Default.Aggregate(result.Cast<double>().ToArray(), (x, y) => x + y);
            return FluxDensity;
        }

        #region Compare
        public int CompareTo(object other, IComparer comparer)
        {
            return (object)this == other ? 1 : 0;
        }

        public int CompareTo(Wire other)
        {
            return Position.CompareTo(other.Position) == 1 ? 1 : 0;
        }

        public int CompareTo(object obj)
        {
            return (object)this == obj ? 1 : 0;
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            var obj = (Wire)other;
            return Position.CompareTo(obj.Position) == 1;
        }

        public bool Equals(Wire other)
        {
            return Position.CompareTo(other.Position) == 1;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return Position.GetHashCode();
        }
        #endregion
    }

    public class WireManager
    {
        public Wire[] Wires { get; private set; }

        public WireManager()
        {
            const int N = 100;
            var positions = Enumerable.Range(0, N).ToArray();
            Wires = new Wire[N];
            for (int i = 0; i < N; ++i)
                Wires[i] = new Wire(Vector3.Create((double)i, 0.0, 10.0));
        }

        public void ComputeFluxDensities(CoilManager coilManager)
        {
            foreach (var wire in Wires)
            {
                wire.ComputeFluxDensity(coilManager);
            }
        }
    }
}
