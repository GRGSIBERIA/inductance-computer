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

        public Coil(Vector3 position, Vector3 forward, Vector3 right, double radius, double height, double sigma = 1.0)
        {
            Position = position;
            Forward = forward;
            Right = right;
            Radius = radius;
            Height = height;
            Sigma = sigma;
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
        public Coil[] Coils { get; private set; }
        public double Gamma { get; private set; }
        public int DivideRadius { get; private set; }
        public int DivideTheta { get; private set; }

        public Vector3[] Fronts
        {
            get
            {
                Vector3[] fronts = new Vector3[Coils.Length];
                for (int i = 0; i < Coils.Length; ++i)
                    fronts[i] = Coils[i].Forward;
                return fronts;
            }
        }

        public Vector3[] Positions
        {
            get
            {
                Vector3[] positions = new Vector3[Coils.Length];
                for (int i = 0; i < Coils.Length; ++i)
                    positions[i] = Coils[i].Position;
                return positions;
            }
        }

        private void MigrateDefaultParameter()
        {
            const int N = 8;
            Coils = new Coil[N];
            for (int i = 0; i < N; ++i)
            {
                Coils[i] = new Coil(
                    Vector3.Create(0.0, (double)i * 2, 0.0), 
                    Vector3.Create(0.0, 0.0, 1.0), 
                    Vector3.Create(1.0, 0.0, 0.0), 1, 1);
            }
        }

        public CoilManager(double gamma = 1.0, int divideRadius = 700, int divideTheta = 720)
        {
            Gamma = gamma;
            DivideRadius = divideRadius;
            DivideTheta = divideTheta;

            MigrateDefaultParameter();
        }
    }
}
