using System;
namespace Tracer
{
    public struct Vec3
    {
        public static readonly Vec3 zero = new Vec3(0.0, 0.0, 0.0);
        public static readonly Vec3 one = new Vec3(1.0, 1.0, 1.0);
        public static readonly Vec3 up = new Vec3(0.0, 1.0, 0.0);
        public static readonly Vec3 right = new Vec3(1.0, 0.0, 0.0);
        public static readonly Vec3 front = new Vec3(0.0, 0.0, -1.0);

        private double x;
        private double y;
        private double z;

        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public double Z { get { return z; } set { z = value; } }
        public double R { get { return x; } set { x = value; } }
        public double G { get { return y; } set { y = value; } }
        public double B { get { return z; } set { z = value; } }

        public double SqrLength { get { return x * x + y * y + z * z; } }
        public double Length { get { return Math.Sqrt(x * x + y * y + z * z); }}

        public Vec3 Normalized{ get { return this / Length; } }

        public Vec3(double e0, double e1, double e2)
        {
            x = e0;
            y = e1;
            z = e2;
        }

        public Vec3(double e)
        {
            x = y = z = e;
        }

        public Vec3(Vec3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        public void Set(double e0, double e1, double e2) 
        {
            x = e0;
            y = e1;
            z = e2;
        }

        public static Vec3 Normalize(Vec3 A)
        {
            return A / A.Length;
        }

        public static double SqrDistance(Vec3 A, Vec3 B) 
        {
            return (A - B).SqrLength;
        }

        public static double Distance(Vec3 A, Vec3 B)
        {
            return (A - B).Length;
        }

        public static Vec3 Reflect(Vec3 vec, Vec3 normal)
        {
            return vec - 2 * Dot(vec, normal.Normalized) * normal.Normalized;
        }

        #region Operators
        public static Vec3 operator +(Vec3 A, Vec3 B)
        {
            Vec3 v;
            v.x = A.x + B.x;
            v.y = A.y + B.y;
            v.z = A.z + B.z;
            return v;
        }

        public static Vec3 operator -(Vec3 A)
        {
            Vec3 v;
            v.x = -A.x;
            v.y = -A.y;
            v.z = -A.z;
            return v;
        }

        public static Vec3 operator -(Vec3 A, Vec3 B)
        {
            Vec3 v;
            v.x = A.x - B.x;
            v.y = A.y - B.y;
            v.z = A.z - B.z;
            return v;
        }

        public static Vec3 operator *(Vec3 A, Vec3 B)
        {
            Vec3 v;
            v.x = A.x * B.x;
            v.y = A.y * B.y;
            v.z = A.z * B.z;
            return v;
        }

        public static Vec3 operator /(Vec3 A, Vec3 B)
        {
            Vec3 v;
            v.x = A.x / B.x;
            v.y = A.y / B.y;
            v.z = A.z / B.z;
            return v;
        }

        public static Vec3 operator +(Vec3 A, double scalar)
        {
            Vec3 v;
            v.x = A.x + scalar;
            v.y = A.y + scalar;
            v.z = A.z + scalar;
            return v;
        }

        public static Vec3 operator -(Vec3 A, double scalar)
        {
            Vec3 v;
            v.x = A.x - scalar;
            v.y = A.y - scalar;
            v.z = A.z - scalar;
            return v;
        }

        public static Vec3 operator *(Vec3 A, double scalar) 
        {
            Vec3 v;
            v.x = A.x * scalar;
            v.y = A.y * scalar;
            v.z = A.z * scalar;
            return v;
        }

        public static Vec3 operator /(Vec3 A, double scalar)
        {
            Vec3 v;
            v.x = A.x / scalar;
            v.y = A.y / scalar;
            v.z = A.z / scalar;
            return v;
        }

        public static Vec3 operator *(double scalar, Vec3 A)
        {
            Vec3 v;
            v.x = A.x * scalar;
            v.y = A.y * scalar;
            v.z = A.z * scalar;
            return v;
        }

        public static double Dot(Vec3 A, Vec3 B) 
        {
            return A.x * B.x + A.y * B.y + A.z * B.z;
        }

        public static Vec3 Cross(Vec3 A, Vec3 B)
        {
            return new Vec3(A.y * B.z - A.z * B.y,
                            A.z * B.x - A.x * B.z,
                            A.x * B.y - A.y * B.x);
        }

        public override String ToString() 
        {
            return "" + x + ' ' + y + ' ' + z;
        }

        #endregion

    }
}
