using System;
using System.IO;

namespace Tracer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            const int height = 500;
            const int width = 1000;

            StreamWriter writer = new StreamWriter("render.ppm");

            writer.WriteLine("P3");
            writer.WriteLine("{0} {1} 255", height, width);
            for (int y = width - 1; y >= 0; y--) {
                for (int x = 0; x < height; x++) {
                    float r = x / height;
                    float g = y / width;
                    float b = 0.2f;
                    int ir = (int)(255.99f * r);
                    int ig = (int)(255.99f * g);
                    int ib = (int)(255.99f * b);
                    writer.WriteLine("{0} {1} {2}", ir, ig, ib);
                }
            }

            writer.Close();
        }
    }
}
