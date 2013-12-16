using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CourseworkGP3
{
    struct Fruit
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
            
           if (position.X > GameConstants.PlayfieldSizeX)
                position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (position.X < -GameConstants.PlayfieldSizeX)
                position.X += 2 * GameConstants.PlayfieldSizeX;
            if (position.Z > GameConstants.PlayfieldSizeZ)
                position.Z -= 2 * GameConstants.PlayfieldSizeZ;
            if (position.Z < -GameConstants.PlayfieldSizeZ)
                position.Z += 2 * GameConstants.PlayfieldSizeZ;
        }
    }
}
