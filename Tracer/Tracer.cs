using System;
using System.IO;
using System.Diagnostics;

namespace Tracer
{
    class MainTracer
    {
        const int maxDepth              = 25;
        const int numSamples            = 100;
        const int width                 = 144;
        const int height                = 90;

        static readonly String filename = "render.ppm";

        static Vec3 bgc                 = new Vec3(0.5, 0.7, 1.0);
        static Random rng               = new Random();

        static Vec3[,] frame            = new Vec3[width, height];

        public static void Main(string[] args)
        {
            HitList world = new HitList();
            world.Add(new Sphere(new Vec3(-4, 1, 0), 1, new Lambertian(new Vec3(0.4, 0.2, 0.1))));

            world.Add(new Sphere(new Vec3(4, 1, 0), 1, new Metallic(new Vec3(0.7, 0.6, 0.5), 0.0)));
            world.Add(new Sphere(new Vec3(0, 1, 0), 1, new Dielectric(1.5)));
            //world.Add(new Sphere(Vec3.front, -0.45, new Dielectric(1.5)));

            Vec3 random = Vec3.zero;
            Vec3 expoint = new Vec3(4.0, 0.2, 0.0);
            for (int i = -11; i < 11; i++)
            {
                for (int j = -11; j < 11; j++)
                {
                    double choose_mat = rng.NextDouble();
                    random.Set(i + 0.9 * rng.NextDouble(), 0.2, j + 0.9 * rng.NextDouble());
                    if (Vec3.Distance(random, expoint) > 0.9)
                    {
                        if (choose_mat < 0.7)
                        {
                            world.Add(new Sphere(random, 0.2, new Lambertian(new Vec3(rng.NextDouble() * rng.NextDouble(), rng.NextDouble() * rng.NextDouble(), rng.NextDouble() * rng.NextDouble()))));
                        }
                        else if (choose_mat < 0.90)
                        {
                            world.Add(new Sphere(random, 0.2, new Metallic(new Vec3(0.5 * (1 + rng.NextDouble()), 0.5 * (1 + rng.NextDouble()), 0.5 * (1 + rng.NextDouble())), 0.5 * rng.NextDouble())));
                        }
                        else
                        {
                            world.Add(new Sphere(random, 0.2, new Dielectric(1.5)));
                        }
                    }
                }
            }

            world.Add(new Sphere(new Vec3(0.0, -1000, -1.0), 1000.0, new Lambertian(Vec3.one * 0.9)));

            Vec3 color;

            Vec3    camPosition = new Vec3(9, 1.95, 2.3);
            double  distToFocus = Vec3.Distance(camPosition, Vec3.front);
            Camera  cam         = new Camera(camPosition, Vec3.front, Vec3.up, 30, width/(double)height, 0.01);

            world.Compute();

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
                    frame[x, y].Set(ir, ig, ib);
                }
                Console.WriteLine("{0} of {1}",height-y, height);

            }

            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine("P3");
            writer.WriteLine("{0} {1} 255", width, height);
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    writer.WriteLine(frame[x, y]);
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
