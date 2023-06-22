using System;

namespace Core.Graphify
{
    [Serializable]
    public struct Vector2D
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2D(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
