using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CourseworkGP3
{
    struct Laser
    {
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public bool isActive;

        public Game1 Game1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void Update(float delta)
        {
            position += direction * speed *
                        GameConstants.LaserSpeedAdjustment * delta;
            if (position.X > GameConstants.PlayfieldSizeX ||
                position.X < -GameConstants.PlayfieldSizeX ||
                position.Z > GameConstants.PlayfieldSizeZ ||
                position.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;
        }
    }
}
