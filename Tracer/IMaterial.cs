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
            scattered = new Ray(hit.hitpoint, target_dir);
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
            scattered = new Ray(hit.hitpoint, target_dir);
            attenuation = albedo;
            return Vec3.Dot(scattered.Direction, hit.normal) > 0;
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
    }
}
