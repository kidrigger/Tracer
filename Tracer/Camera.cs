using System;
namespace Tracer
{
    public class Camera
    {
        private Vec3 lowerLeftCorner;
        private Vec3 horizontal;
        private Vec3 vertical;
        private Vec3 position;

        private Vec3 w;
        private Vec3 u;
        private Vec3 v;

        private RandomSphere rsg;

        private double lensRadius;

        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 viewUp, double fov, double aspect, double aperture = 0.0, double focalLength = -1.0)
        {
            double theta = Math.PI * fov / 180;
            double half_height = Math.Tan(theta / 2);
            double half_width = aspect * half_height;

            lensRadius = aperture / 2;
            if (focalLength < 0)
            {
                focalLength = Vec3.Distance(lookFrom, lookAt);
            }

            position = lookFrom;
            w = Vec3.Normalize(lookFrom - lookAt);
            u = Vec3.Normalize(Vec3.Cross(viewUp, w));
            v = Vec3.Cross(w, u);

            lowerLeftCorner = position - (half_width * u + half_height * v + w) * focalLength;
            horizontal = 2 * half_width * focalLength * u;
            vertical = 2 * half_height * focalLength * v;

            rsg = new RandomSphere();
        }

        public Ray GetRay(double s, double t)
        {
            Vec3 diskPoint = rsg.GetRandomPointInUnitDisk() * lensRadius;
            Vec3 offset = u * diskPoint.X + v * diskPoint.Y;
            return new Ray(position + offset, lowerLeftCorner + s * horizontal + t * vertical - position - offset);
        }
    }
}
