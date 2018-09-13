using System;
using System.Collections.Generic;

namespace Tracer
{
    public class BVHNode : IHitable
    {
        public int start;
        public int length;
        private AABB box;
        private IHitable left;
        private IHitable right;

        public BVHNode(List<IHitable> world, int start, int length)
        {
            this.start = start;
            this.length = length;

            Random rng = new Random();
            int axis = rng.Next()%3;

            IComparer<IHitable> sorter;

            switch (axis)
            {
                case 0: sorter = new SortByX(); break;
                case 1: sorter = new SortByY(); break;
                default: sorter = new SortByZ(); break;
            }

            world.Sort(start, length, sorter);

            if (length == 1) {
                left = right = world[start];
            } else if (length == 2) {
                left = world[start];
                right = world[start + 1];
            } else {
                left  = new BVHNode(world, start, length / 2);
                right = new BVHNode(world, start + length / 2, length - length/2);
            }

            box = AABB.CalculateSurroundBox(left.BoundingBox(), right.BoundingBox());
        }

        public AABB BoundingBox()
        {
            return box;
        }

        public bool Hit(Ray ray, double tmin, double tmax, out HitInfo hit)
        {
            bool hitLeft  = left.Hit(ray, tmin, tmax, out HitInfo leftHitInfo);
            bool hitRight = right.Hit(ray, tmin, tmax, out HitInfo rightHitInfo);

            if (hitLeft && hitRight)
            {
                hit = leftHitInfo.t < rightHitInfo.t ? leftHitInfo : rightHitInfo;
                return true;
            }
            if (hitLeft)
            {
                hit = leftHitInfo;
                return true;
            }
            if (hitRight)
            {
                hit = rightHitInfo;
                return true;
            }
            hit = leftHitInfo;
            return false;
        }
    }

    class SortByX : IComparer<IHitable>
    {
        public int Compare(IHitable A, IHitable B)
        {
            AABB a = A.BoundingBox();
            AABB b = B.BoundingBox();
            int axis = 0;

            return a.Min[axis] < b.Min[axis] ? -1 : a.Min[axis] < b.Min[axis] ? 1 : 0;
        }
    }

    class SortByY : IComparer<IHitable>
    {
        public int Compare(IHitable A, IHitable B)
        {
            AABB a = A.BoundingBox();
            AABB b = B.BoundingBox();
            int axis = 1;

            return a.Min[axis] < b.Min[axis] ? -1 : a.Min[axis] < b.Min[axis] ? 1 : 0;
        }
    }

    class SortByZ : IComparer<IHitable>
    {
        public int Compare(IHitable A, IHitable B)
        {
            AABB a = A.BoundingBox();
            AABB b = B.BoundingBox();
            int axis = 2;

            return a.Min[axis] < b.Min[axis] ? -1 : a.Min[axis] < b.Min[axis] ? 1 : 0;
        }
    }
}
