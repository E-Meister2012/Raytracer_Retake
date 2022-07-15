using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Template
{
    internal class Raytracer
    {
        Vector3 upLeft;
        Vector3 upRight;
        Vector3 downLeft;
        Ray ray = new Ray();
        Scene scene;
        Camera camera;
        Intersection intersection = new Intersection();
        //public bool raytracing(Triangle t, Vector3 P)
        //{
        //    if (Vector3.Cross(t.B - t.A, P - t.A).Length / 2 < 0) return false;
        //    if (Vector3.Cross(t.C - t.B, P - t.B).Length / 2 < 0) return false;
        //    if (Vector3.Cross(t.A - t.C, P - t.C).Length / 2 < 0) return false;
        //    //Area XYZ = || (Z - Y) * (X - Y) || / 2
        //    //shading normal = aA * nA + aB * nB + aC * nC;
        //    return true;

        //}
        internal void Render()
        {
            //the width of the plane
            Vector3 horizon = upRight - upLeft;
            //the height of the plane
            Vector3 vertical = upLeft - downLeft;
            //set the pos of the camera
            ray.origin = camera.position;
            for (int y = 0; y < OpenTKApp.app.screen.height; y++)
            {
                for (int x = 0; x < OpenTKApp.app.screen.width; x++)
                {
                    //reset the primary ray and set the direction and normalize it
                    ray.scalar = 0;
                    Intersection near = new Intersection();
                    Vector3 RGB = new Vector3(0);
                    near.ray.scalar = float.MaxValue;
                    ray.direction = upLeft - (x / OpenTKApp.app.screen.width) * horizon + (y / OpenTKApp.app.screen.height) * vertical;
                    ray.direction.Normalize();
                    //for every object
                    foreach (Primitive p in scene.primitives)
                    {
                        if (Intersects((Triangle)p, ref ray))
                        {
                            if (ray.scalar < near.ray.scalar)
                            {
                                near.ray = ray;
                                near.lastHit = p;
                            }
                        }
                    }
                    if (near.lastHit != null)
                        RGB = LightColor(near.ray, (Triangle)near.lastHit);
                    OpenTKApp.app.screen.Plot(x, y, CalcColor(
                    (int)(MathHelper.Clamp(RGB.X, 0f, 1f) * 255),
                    (int)(MathHelper.Clamp(RGB.Y, 0f, 1f) * 255),
                    (int)(MathHelper.Clamp(RGB.Z, 0f, 1f) * 255)));

                }
            }
        }
        public Vector3 LightColor(Ray r, Triangle t)
        {
            Ray shadow = new Ray();
            shadow.origin = r.origin+ r.direction * r.scalar;
            foreach(Light l in scene.lights)
            {
                shadow.scalar = 0;
                shadow.direction = l.location - shadow.origin;
                shadow.direction.Normalize();
                bool gotHit = false;
                foreach (Primitive p in scene.primitives)
                {
                    gotHit = Intersects((Triangle)p, ref shadow);
                    if (gotHit)
                        break;
                }

                  r.RGB += l.returnColor(t.Normal, shadow.direction, new Vector3(1, 1, 0));
            }
            return r.RGB + new Vector3(0.07f);
        }
        public bool Intersects(Triangle p, ref Ray ray)
        {
            if (intersectTriangle((Triangle)p, ref ray))
            {
                intersection.ray = ray;
                intersection.lastHit = p;
                return true;
            }
            else return false;
        }
        public bool intersectTriangle(Triangle p, ref Ray ray)
        {
            if (Vector3.Dot(ray.direction, p.Normal) <= 0.00001)
            if (Vector3.Dot(ray.direction, p.Normal) <= 0.00001)
                return false;
            float scalar = -(Vector3.Dot(p.Normal, ray.origin) - Vector3.Dot(p.Normal, p.A)) / Vector3.Dot(p.Normal, ray.direction);
            if (Math.Abs(scalar) <= 0.00001)
                return false;
            ray.scalar = scalar;
                return (Vector3.Dot(Vector3.Cross(p.B - p.A, ray.origin + ray.direction * ray.scalar - p.A), p.Normal) < 0)
                && (Vector3.Dot(Vector3.Cross(p.A - p.C, ray.origin + ray.direction * ray.scalar - p.C), p.Normal) < 0)
                    && (Vector3.Dot(Vector3.Cross(p.C - p.B, ray.origin + ray.direction * ray.scalar - p.B), p.Normal) < 0);

        }
        internal int CalcColor(int red, int green, int blue)
        {
            return (red << 16) + (green << 8) + blue;
        }

        internal struct Ray
        {
            public Vector3 origin;
            public Vector3 direction;
            public Vector3 RGB;
            public float scalar;
        }
        public Raytracer(Scene scene, Camera camera)
        {
            this.scene = scene;
            this.camera = camera;
            ray.origin = camera.position;

            //calculating the edge points of the plane
            Vector3 center = camera.position + camera.lookAtDirection;
            Vector3 rightDirection = Vector3.Cross(camera.upDirection, camera.lookAtDirection);
            upLeft = center + camera.upDirection - rightDirection;
            upRight = center + camera.upDirection + rightDirection;
            downLeft = center - camera.upDirection - rightDirection;
        }


    }
}
