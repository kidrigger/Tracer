using System;
namespace Tracer
{
    public class Sphere : IHitable
    {
        private Vec3 center;
        private double radius;
        private IMaterial material;

        public Sphere(Vec3 center, double radius, IMaterial material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        public bool Hit(Ray ray, double tmin, double tmax, out HitInfo hit)
        {
            bool isHit = HitCheck(ray, out double sol1, out double sol2);

            if (sol1 < tmax && sol1 > tmin)
            {
                hit.t = sol1;
            }
            else if (sol2 < tmax && sol2 > tmin)
            {
                hit.t = sol2;
            }
            else
            {
                isHit = false;
                hit.t = 0;
            }

            hit.hitpoint = ray.PointAtParam(hit.t);
            hit.normal = (hit.hitpoint - center)/radius;
            hit.material = material;
            return isHit;
        }

        private bool HitCheck(Ray ray, out double sol1, out double sol2) 
        {
            double a = ray.B.SqrLength;
            double b = 2 * Vec3.Dot(ray.B, ray.A - center);
            double c = Vec3.SqrDistance(ray.A, center) - radius * radius;

            double discriminant = b * b - 4 * a * c;
            bool hit = discriminant >= 0;

            if (hit) 
            {
                discriminant = Math.Sqrt(discriminant);
                sol1 = (-b - discriminant) / (2 * a);
                sol2 = (-b + discriminant) / (2 * a);
            } else {
                sol1 = Double.NaN;
                sol2 = Double.NaN;
            }
            return hit;
        }
    }
}
