using System;
using System.IO;
using System.Diagnostics;

namespace Tracer
{
    class MainTracer
    {
        const int maxDepth              = 50;
        const int numSamples            = 100;
        const int width                 = 1440;
        const int height                = 900;

        static readonly String filename = "render.ppm";

        static Vec3 bgc                 = new Vec3(0.5, 0.7, 1.0);
        static Random rng               = new Random();

        public static void Main(string[] args)
        {
            HitList world = new HitList();
            world.Add(new Sphere(Vec3.front, 0.5, new Lambertian(new Vec3(0.6, 0.0, 0.0))));

            world.Add(new Sphere(Vec3.front + Vec3.right, 0.5, new Metallic(new Vec3(1.0, 1.0, 0.4) * 0.7, 0.3)));
            world.Add(new Sphere(Vec3.front - Vec3.right, 0.5, new Metallic(Vec3.one * 0.8 , 0.0)));

            world.Add(new Sphere(new Vec3(0.0, -100.5, -1.0), 100.0, new Lambertian(Vec3.one * 0.9)));

            Vec3 color;

            StreamWriter writer = new StreamWriter(filename);
            Camera cam          = new Camera(width, height);

            writer.WriteLine("P3");
            writer.WriteLine("{0} {1} 255", width, height);

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    color = Vec3.zero;
                    for (int i = 0; i < numSamples; i++)
                    {
                        double u = (x + rng.NextDouble()) / width;
                        double v = (y + rng.NextDouble()) / height;

                        color += Color(cam.GetRay(u, v), world);
                    }
                    color /= numSamples;

                    int ir = (int)(255.99 * Math.Sqrt(color.R));
                    int ig = (int)(255.99 * Math.Sqrt(color.G));
                    int ib = (int)(255.99 * Math.Sqrt(color.B));
                    writer.WriteLine("{0} {1} {2}", ir, ig, ib);
                }
                if (y % (height / 10) == 0)
                {
                    Console.WriteLine("{0} percent done.", (int)((100.99 * (height - y)) / height));
                }
            }

            writer.Close();
            Console.WriteLine("Written to " + filename);

            Process.Start("open", filename);
        }

        private static Vec3 Color(Ray ray, IHitable geometry, int depth = 0) 
        {
            if (geometry.Hit(ray, 0.001, Double.MaxValue, out HitInfo hit))
            {
                if (depth < maxDepth && hit.material.Scatter(ray, hit, out Vec3 attenuation, out Ray scattered))
                {
                    return attenuation * Color(scattered, geometry, depth + 1);
                }
                else
                {
                    return Vec3.zero;
                }
            }
            double par = 0.5 * (ray.Direction.Normalized.Y + 1.0);
            return (1.0 - par) * Vec3.one + par * bgc;
        }
    }
}
