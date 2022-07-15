using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    internal class Light
    {
        internal Vector3 location;
        internal Vector3 intensity;
        Vector3 RGB;
        internal int gloss;
        public Light(Vector3 location, Vector3 intensity, Vector3 RGB, int gloss = 1)
        {
            this.location = location;
            this.intensity = intensity;
            this.gloss = gloss;
            this.RGB = RGB;
        }
        internal Vector3 returnColor(Vector3 normal, Vector3 lightDirection, Vector3 colour)
        {
            Vector3 reflected = (1f / lightDirection.LengthSquared) * intensity * colour * RGB * Math.Max(0, Vector3.Dot(normal, lightDirection));
            return Vector3.Clamp(new Vector3(0.07f), reflected, new Vector3(1));
        }

    }
}
