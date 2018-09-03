using System;
namespace Tracer
{
    public interface IMaterial
    {
        bool Scatter(Ray ray, HitInfo hit, out Vec3 attenuation, out Ray scattered);
    }

    public class Lambertian : IMaterial
    {
        public Vec3 albedo;
        private static RandomSphere rng;

        public Lambertian(Vec3 albedo) 
        {
            if (rng == null)
            {
                rng = new RandomSphere();
            }
            this.albedo = albedo;
        }

        public bool Scatter(Ray ray, HitInfo hit, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 target_dir = hit.normal + rng.GetRandomPointInUnitSphere();
            scattered.A = hit.hitpoint;
            scattered.B = target_dir;
            attenuation = albedo;
            return true;
        }
    }

    public class Metallic : IMaterial
    {
        public Vec3 albedo;
        public double fuzziness;
        private static RandomSphere rng;

        public Metallic(Vec3 albedo, double fuzziness)
        {
            if (rng == null)
            {
                rng = new RandomSphere();
            }
            this.albedo = albedo;
            this.fuzziness = fuzziness;
        }

        public bool Scatter(Ray ray, HitInfo hit, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 target_dir = Vec3.Reflect(ray.Direction, hit.normal) + rng.GetRandomPointInUnitSphere() * fuzziness;
            scattered.A = hit.hitpoint;
            scattered.B = target_dir;
            attenuation = albedo;
            return Vec3.Dot(scattered.Direction, hit.normal) > 0;
        }
    }

    public class Dielectric : IMaterial
    {
        public Vec3 albedo;
        public double ri;
        private static Random rng;

        public Dielectric(double refractiveIndex)
        {
            albedo = Vec3.one;
            ri = refractiveIndex;
            if (rng == null)
                rng = new Random();
        }

        public Dielectric(Vec3 albedo, double refractiveIndex)
        {
            this.albedo = albedo;
            ri = refractiveIndex;
            if (rng == null)
                rng = new Random();

        }

        public bool Scatter(Ray ray, HitInfo hit, out Vec3 attenuation, out Ray scattered)
        {
            attenuation = albedo;

            double nu;
            double cosine;
            Vec3 norm;
            Vec3 reflected = Vec3.Reflect(ray.Direction, hit.normal);

            double reflect_prob;

            if (Vec3.Dot(hit.normal, ray.Direction) > 0)
            {
                norm = -hit.normal;
                nu = ri;
                cosine = ri * Vec3.Dot(ray.Direction, hit.normal)/ray.Direction.Length;
            }
            else
            {
                norm = hit.normal;
                nu = 1.0 / ri;
                cosine = -Vec3.Dot(ray.Direction, hit.normal)/ray.Direction.Length;
            }

            reflect_prob = Refract(ray.Direction, norm, nu, out Vec3 refracted) ? SchlickReflectivity(cosine) : 1.0;

            scattered.A = hit.hitpoint;
            scattered.B = reflect_prob > rng.NextDouble() ? reflected : refracted;
            return true;
        }

        private bool Refract(Vec3 v, Vec3 n, double nu, out Vec3 refracted)
        {
            Vec3 uv = v.Normalized;
            double dt = Vec3.Dot(uv, n);

            double discriminant = 1.0 - nu * nu * (1 - dt * dt);

            if (discriminant > 0)
            {
                refracted = nu * (uv - n * dt) - n * Math.Sqrt(discriminant);
                return true;
            }
            refracted = v;
            return true;
        }

        private double SchlickReflectivity(double cosine)
        {
            double r0 = (1 - ri) / (1 + ri);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
    }

    class RandomSphere 
    {
        Random rng;

        public RandomSphere()
        {
            rng = new Random();
        }

        public Vec3 GetRandomPointInUnitSphere()
        {
            Vec3 point = Vec3.zero;
            do
            {
                point.Set(rng.NextDouble() * 2.0 - 1.0, rng.NextDouble() * 2.0 - 1.0, rng.NextDouble() * 2.0 - 1.0);
            }
            while (point.SqrLength > 1.0);
            return point;
        }

        public Vec3 GetRandomPointInUnitDisk()
        {
            Vec3 point = Vec3.zero;
            do
            {
                point.Set(rng.NextDouble() * 2.0 - 1.0, rng.NextDouble() * 2.0 - 1.0, 0.0);
            }
            while (point.SqrLength > 1.0);
            return point;
        }
    }
}
