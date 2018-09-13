using System;
using System.Collections.Generic;

namespace Tracer
{
    public class HitList : IHitable
    {
        private List<IHitable> hitables;
        private BVHNode bvhRoot;

        public HitList()
        {
            hitables = new List<IHitable>();
        }

        public bool Hit(Ray ray, double tmin, double tmax, out HitInfo hit)
        {
            return bvhRoot.Hit(ray, tmin, tmax, out hit);
        }

        public void Add(IHitable obj)
        {
            hitables.Add(obj);
        }

        public void Compute() 
        {
            bvhRoot = new BVHNode(hitables, 0, hitables.Count);
        }

        public AABB BoundingBox()
        {
            return bvhRoot.BoundingBox();
        }
    }
}
