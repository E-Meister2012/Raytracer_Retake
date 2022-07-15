using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    internal class Scene
    {
        internal List<Light> lights = new List<Light>();
        internal List<Primitive> primitives = new List<Primitive>();
        public Scene() { }
    }
}
