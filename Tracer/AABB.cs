using System;
namespace Tracer
{
    public struct AABB
    {
        public Vec3 Min;
        public Vec3 Max;

        public AABB(Vec3 a, Vec3 b)
        {
            Min = a;
            Max = b;
        }

        public bool Hit(Ray ray, double tmin, double tmax)
        {
            for (int i = 0; i < 3; i++) {
                double invD = 1.0 / ray.Direction[i];
                double t0   = Math.Min((Min[i] - ray.Origin[i])*invD, (Max[i] - ray.Origin[i])*invD);
                double t1   = Math.Max((Min[i] - ray.Origin[i])*invD, (Max[i] - ray.Origin[i])*invD);

                if (invD < 0.0f) {
                    invD = t0;
                    t0   = t1;
                    t1   = invD;
                }

                tmin = t0 > tmin ? t0 : tmin;
                tmax = t1 < tmax ? t1 : tmax;

                if (tmax <= tmin) {
                    return false;
                }
            }
            return true;
        }

        public static AABB CalculateSurroundBox(AABB b0, AABB b1)
        {
            AABB bb;
            Vec3 min = Vec3.zero;
            Vec3 max = Vec3.zero;

            for (int i = 0; i < 3; i++) {
                min[i] = b0.Min[i] > b1.Min[i] ? b0.Min[i] : b1.Min[i];
                max[i] = b0.Max[i] > b1.Max[i] ? b0.Max[i] : b1.Max[i];
            }

            bb.Min = min;
            bb.Max = max;
            return bb;
        }
    }
}
