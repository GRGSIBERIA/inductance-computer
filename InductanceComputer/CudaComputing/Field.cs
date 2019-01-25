using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;

namespace CudaComputing
{
    public class Field
    {
        private double FluxDensityOfMeasurePoint(Vector3 measurePoint, double[] fluxies, Vector3[] wirePositions, Vector3[] fronts, double gamma)
        {
            double flux = 0.0;
            
            for (int wireId = 0; wireId < fluxies.Length; ++wireId)
            {
                double left = gamma * fluxies[wireId];

                // 測定点は複数の強磁性体によって誘導される
                double totalFracUp = 0.0;
                for (int coilId = 0; coilId < fronts.Length; ++coilId)
                {
                    totalFracUp += (Vector3.Dot(fronts[coilId], (wirePositions[wireId] - measurePoint)) * fronts[coilId]).Length;
                }

                double fracDown = (wirePositions[wireId] - measurePoint).Length;
                fracDown = fracDown * fracDown * fracDown;

                flux += left * (totalFracUp / fracDown);
            }

            return flux;
        }

        /// <summary>
        /// 空間の磁束密度を求める
        /// </summary>
        /// <param name="fieldSize">空間の大きさ</param>
        /// <param name="partitionSize">空間の分割数</param>
        /// <param name="coilManager"></param>
        /// <param name="wireManager"></param>
        /// <returns></returns>
        [GpuManaged]
        public double[,,] FieldFluxDensity(Vector3 fieldSize, int3 partitionSize, CoilManager coilManager, WireManager wireManager)
        {
            var fieldFluxDensity = new double[partitionSize.x, partitionSize.y, partitionSize.z];
            var fluxies = Gpu.Default.Allocate(wireManager.FluxDensities);
            var fronts = Gpu.Default.Allocate(coilManager.Fronts);
            var wirePositions = Gpu.Default.Allocate(wireManager.Positions);
            var gamma = coilManager.Gamma;

            // インクリメントあたりの大きさ
            var incrementSize = Vector3.Create(fieldSize.x / partitionSize.x, fieldSize.y / partitionSize.y, fieldSize.z / partitionSize.z);

            for (int zi = 0; zi < partitionSize.z; ++zi)
            {
                for (int yi = 0; yi < partitionSize.y; ++yi)
                {
                    Gpu.Default.For(0, partitionSize.x, xi =>
                    {
                        // 測定点の磁束密度を求める
                        var measurePoint = Vector3.Create(xi, yi, zi) * incrementSize;  // 測定点

                        fieldFluxDensity[xi, yi, zi] = FluxDensityOfMeasurePoint(measurePoint, fluxies, wirePositions, fronts, gamma);
                    });
                }
            }

            Gpu.Free(fluxies);
            Gpu.Free(fronts);
            Gpu.Free(wirePositions);
            Gpu.Default.Synchronize();

            return fieldFluxDensity;
        }

        public double Inductance(double[,,] previeousInductance, double deltaTime, Vector3 size, int3 partitionSize)
        {
            Vector3 divisionSize = size / partitionSize;


            return 0.0;
        }
    }
}
