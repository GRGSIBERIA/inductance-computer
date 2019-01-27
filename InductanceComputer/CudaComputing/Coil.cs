using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CudaComputing
{
    public struct Coil : IEquatable<Coil>, IStructuralEquatable, IComparable<Coil>, IComparable, IStructuralComparable
    {
        public Vector3 Position { get; private set; }
        public Vector3 Forward { get; private set; }
        public Vector3 Right { get; private set; }
        public double Radius { get; private set; }
        public double Height { get; private set; }
        public double Sigma { get; private set; }
        public double Gamma { get; private set; }

        public Coil(Vector3 position, Vector3 forward, Vector3 right, double radius, double height, double sigma = 1.0, double gamma = 1.0)
        {
            Position = position;
            Forward = forward;
            Right = right;
            Radius = radius;
            Height = height;
            Sigma = sigma;
            Gamma = gamma;
        }

        public static Coil Create(Vector3 position, Vector3 forward, Vector3 right, double radius, double height, double sigma = 1.0)
        {
            var coil = new Coil();
            coil.Position = position;
            coil.Forward = forward;
            coil.Right = right;
            coil.Radius = radius;
            coil.Height = height;
            coil.Sigma = sigma;
            return coil;
        }

        #region Compare
        public int CompareTo(object other, IComparer comparer)
        {
            return (object)this == other ? 1 : 0;
        }

        public int CompareTo(Coil other)
        {
            return Position.CompareTo(other.Position) == 1 ? 1 : 0;
        }

        public int CompareTo(object obj)
        {
            return (object)this == obj ? 1 : 0;
        }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            var obj = (Coil)other;
            return Position.CompareTo(obj.Position) == 1;
        }

        public bool Equals(Coil other)
        {
            return Position.CompareTo(other.Position) == 1;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return Position.GetHashCode();
        }
        #endregion
    }

    public class CoilManager
    {
        public Coil[,] Coils { get; private set; }
        public int DivideRadius { get; private set; }
        public int DivideTheta { get; private set; }
        public int CoilCount { get; private set; }

        public Vector3[] Fronts(int coilIndex)
        {
            Vector3[] fronts = new Vector3[Coils.Length];
            for (int i = 0; i < Coils.Length; ++i)
                fronts[i] = Coils[coilIndex, i].Forward;
            return fronts;
        }

        public Vector3[] Positions(int coilIndex)
        {
            Vector3[] positions = new Vector3[Coils.Length];
            for (int i = 0; i < Coils.Length; ++i)
                positions[i] = Coils[coilIndex, i].Position;
            return positions;
        }

        private void MigrateDefaultParameter()
        {
            const int N = 8;
            Coils = new Coil[1, N];
            for (int i = 0; i < N; ++i)
            {
                Coils[0, i] = new Coil(
                    Vector3.Create(0.0, (double)i * 2, 0.0),
                    Vector3.Create(0.0, 0.0, 1.0),
                    Vector3.Create(1.0, 0.0, 0.0), 1, 1);
            }
            CoilCount = 1;
        }

        public CoilManager(double gamma = 1.0, int divideRadius = 700, int divideTheta = 720)
        {
            DivideRadius = divideRadius;
            DivideTheta = divideTheta;

            MigrateDefaultParameter();
        }

        /// <summary>
        /// コイルを初期化する
        /// </summary>
        /// <param name="coilCount">コイルの数</param>
        /// <param name="timeCount">時間の数</param>
        public void InitializeCoilData(int coilCount, int timeCount)
        {
            CoilCount = coilCount;
            Coils = new Coil[coilCount, timeCount];
        }

        public void RecieveCoilData(int coilIndex, int timeCount, Vector3[] positions, Vector3[] forwards, Vector3[] rights, double radius, double height, double sigma = 1.0, double gamma = 1.0)
        {
            for (int i = 0; i < timeCount; ++i)
                Coils[coilIndex, i] = new Coil(positions[i], forwards[i], rights[i], radius, height, sigma, gamma);
        }
    }
}
