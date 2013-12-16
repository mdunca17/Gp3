using System;
using System.Collections.Generic;
using System.Text;

namespace CourseworkGP3
{
    static class GameConstants
    {
        //camera constants
        public const float CameraHeight = 25000.0f;
        public const float PlayfieldSizeX = 48f;
        public const float PlayfieldSizeZ = 34f;
        //Dalek constants
        public const int NumFruit = 10;
        public const float FruitMinSpeed = 1.0f;
        public const float FruitMaxSpeed = 1.0f;
        public const float FruitSpeedAdjustment = 1.0f;
        public const float FruitScalar = 0.01f;

        public const int NumBirds = 6;
        public const float BirdsMinSpeed = 5.0f;
        public const float BirdsMaxSpeed = 5.0f;
        public const float BirdsSpeedAdjustment = 1.0f;
        public const float BirdsScalar = 0.01f;
        //collision constants
        public const float FruitBoundingSphereScale = 0.025f;  //50% size
        public const float SnakeBoundingSphereScale = 0.3f;  //50% size
        public const float LaserBoundingSphereScale = 0.85f;  //50% size
        public const float HedgeBoundingSphereScale = 0.85f;
        public const float BirdsBoundingSphereScale = 0.008f;

        //bullet constants
        public const int NumLasers = 30;
        public const float LaserSpeedAdjustment = 5.0f;
        public const float LaserScalar = 3.0f;

        public static Game1 Game1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

    }
}
