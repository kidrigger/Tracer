using System;
namespace Tracer
{
    public class Camera
    {
        private Vec3 lowerLeftCorner;
        private Vec3 horizontal;
        private Vec3 vertical;
        private Vec3 position;

        public Camera(double width, double height)
        {
            double aspect = width / height;
            lowerLeftCorner = new Vec3(-1.0 * aspect, -1.0, -1.0);
            horizontal = new Vec3(2.0 * aspect, 0.0, 0.0);
            vertical = new Vec3(0.0, 2.0, 0.0);
            position = Vec3.zero;
        }

        public Ray GetRay(double u, double v)
        {
            return new Ray(position, lowerLeftCorner + u * horizontal + v * vertical);
        }
    }
}
