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
        float alpha, beta, gamma;
        Vector3 upLeft;
        Vector3 upRight;
        Vector3 downLeft;
        Ray ray = new Ray();
        Scene scene = new Scene();
        Camera camera = new Camera(Vector3.Zero, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 0);
        public bool raytracing(Triangle t, Vector3 P)
        {
            if (Vector3.Cross(t.B - t.A, P - t.A).Length / 2 < 0) return false;
            if (Vector3.Cross(t.C - t.B, P - t.B).Length / 2 < 0) return false;
            if (Vector3.Cross(t.A - t.C, P - t.C).Length / 2 < 0) return false;
            //Area XYZ = || (Z - Y) * (X - Y) || / 2
            //shading normal = aA * nA + aB * nB + aC * nC;
            return true;

        }
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
                    ray.direction = upLeft + ((float)x / (float)OpenTKApp.app.screen.width) * horizon + ((float)y / (float)OpenTKApp.app.screen.height) * vertical;
                    ray.direction.Normalize();
                    //for every object
                    foreach (Primitive p in scene.primitives)
                    {
                        ray.scalar = 1;
                        if(p is Triangle)
                        if (Intersects((Triangle)p, ref ray))
                        {
                                ray.RGB = LightColor(ray, (Triangle)p);
                        }
                    }
                    OpenTKApp.app.screen.Plot(x, y, CalcColor((int)(MathHelper.Clamp(ray.RGB.X, 0, 1) * 255),
                    (int)(MathHelper.Clamp(ray.RGB.Y, 0, 1) * 255),
                    (int)(MathHelper.Clamp(ray.RGB.Z, 0, 1) * 255)));

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
                if (!gotHit)
                {
                    r.RGB += l.returnColor(t.Normal, shadow.direction, new Vector3(1, 1, 0));
                }
                else
                    r.RGB = new Vector3(0.07f);
            }
            return r.RGB + new Vector3(0.07f);
        }
        public bool Intersects(Triangle p, ref Ray ray)
        {
            Vector3 v = ray.direction - ray.origin;
            Plane plane = new Plane(p.Normal, p.A, new Vector3(1));
            Vector3 w = plane.point - ray.origin;
            float area = Vector3.Cross(p.C - p.B, p.A - p.B).Length / 2;
            ray.scalar = Vector3.Dot(w, p.Normal) / Vector3.Dot(v, p.Normal);
            if(ray.scalar < 0 && ray.scalar > 1)
                return false;
            if (Vector3.Cross(p.C - p.B, ray.origin + ray.direction * ray.scalar - p.B).Length / 2 < 0)
                return false;
            alpha = Vector3.Cross(p.C - p.B, ray.origin + ray.direction * ray.scalar - p.B).Length / 2;
            beta = Vector3.Cross(p.A - p.C, ray.origin + ray.direction * ray.scalar - p.C).Length / 2;
            gamma = 1 - beta - alpha;
            return true;
        }
        internal int CalcColor(int red, int green, int blue)
        {
            return (red << 16) + (blue << 8) + blue;
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
            Vector3 center = camera.position + camera.lookAtDirection * camera.FOV;
            Vector3 rightDirection = Vector3.Cross(camera.upDirection, camera.lookAtDirection);
            upLeft = center + camera.upDirection - rightDirection;
            upRight = center + camera.upDirection + rightDirection;
            downLeft = center - camera.upDirection - rightDirection;
        }


    }
}
