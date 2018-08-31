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
            foreach (IHitable obj in hitables) {
                if (obj.Hit(ray, tmin, tmax, out hit)) return true;
            }
            hit.t = -1;
            hit.hitpoint = Vec3.zero;
            hit.normal = Vec3.zero;
            hit.material = null;
            return false;
        }

        public void Add(IHitable obj)
        {
            hitables.Add(obj);
        }
    }
}
