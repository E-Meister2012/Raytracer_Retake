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
        public Camera(Vector3 position, Vector3 lookAtDirection, Vector3 upDirection, float angle = 120)
        {
            this.position = position;
            this.lookAtDirection = lookAtDirection;
            lookAtDirection.Normalize();
            this.upDirection = upDirection;
            upDirection.Normalize();
            FOV = 1/(float)Math.Tan(MathHelper.DegreesToRadians(angle / 2));
        }
    }
}
