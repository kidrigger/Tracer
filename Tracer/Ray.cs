using System;
namespace Tracer
{
    public struct Ray
    {
        public Vec3 A;
        public Vec3 B;

        public Ray(Vec3 origin, Vec3 direction) {
            A = origin;
            B = direction;
        }

        public void Set(Vec3 origin, Vec3 direction)
        {
            A = origin;
            B = direction;
        }

        public Vec3 Origin { get { return A; } }
        public Vec3 Direction { get { return B; } }

        public Vec3 PointAtParam(double t) {
            return A + t * B;
        }
    }
}
