using System;
using System.Collections.Generic;

namespace Tracer
{
    public class HitList : IHitable
    {
        private List<IHitable> hitables;

        public HitList()
        {
            hitables = new List<IHitable>();
        }

        public bool Hit(Ray ray, double tmin, double tmax, out HitInfo hit)
        {
            hit.t = Double.PositiveInfinity;
            hit.hitpoint = Vec3.zero;
            hit.normal = Vec3.zero;
            hit.material = null;
            foreach (IHitable obj in hitables) {
                if (obj.Hit(ray, tmin, tmax, out HitInfo hitRecord)) 
                {
                    if (hit.t > hitRecord.t)
                    {
                        hit = hitRecord;
                    }
                }
            }
            return !Double.IsPositiveInfinity(hit.t);
        }

        public void Add(IHitable obj)
        {
            hitables.Add(obj);
        }
    }
}
