using System;
namespace Tracer
{
    public struct HitInfo {
        public Vec3 hitpoint;
        public Vec3 normal;
        public double t;
        public IMaterial material;
    }

    public interface IHitable
    {
        bool Hit(Ray ray, double tmin, double tmax, out HitInfo hit);
    }
}
