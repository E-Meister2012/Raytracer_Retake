using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    internal class Primitive
    {
        Vector3 RGB;
        public Primitive(Vector3 rgb)
        {
            RGB = rgb;
        }

    }
    internal class Plane : Primitive
    {
        internal Vector3 normal;
        internal Vector3 point;
        public Plane(Vector3 normal, Vector3 point, Vector3 rgb): base(rgb)
        {
            this.normal = normal;
            this.normal.Normalize();
            this.point = point;
        }
    }
    internal class Triangle : Primitive
    {
        public Vector3 A, B, C;
        public Vector3 Normal;
        public Vector3 uvA, uvB, uvC;
        public Triangle(Vector3 a, Vector3 b, Vector3 c, Vector3 rgb) : base(rgb)
        {
            A = a;
            B = b;
            C = c;
            Normal = Vector3.Cross(B - A, C - A);
            Normal.Normalize();
        }
    }
}
