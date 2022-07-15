using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK;

namespace Template
{
    internal class Camera
    {
        public Vector3 position;
        public Vector3 lookAtDirection;
        public Vector3 upDirection;
        public float FOV;
        internal Plane screen;
        public Camera(Vector3 position, Vector3 lookAtDirection, Vector3 upDirection, float angle)
        {
            this.position = position;
            this.lookAtDirection = lookAtDirection;
            this.upDirection = upDirection;
            Vector3 rightDirection =Vector3.Cross(upDirection, lookAtDirection); 
            this.FOV = 1/(float)Math.Tan(MathHelper.DegreesToRadians(angle / 2));
            screen = new Plane(position + lookAtDirection, position + lookAtDirection * FOV, new Vector3(0));

        }
    }
}
