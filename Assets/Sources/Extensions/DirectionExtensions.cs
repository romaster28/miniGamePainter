using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Core;

namespace Sources.Extensions
{
    public static class DirectionExtensions
    {
        private const float FaultValue = 10;
        
        private static readonly Dictionary<Direction, float> RotationToDirection = new Dictionary<Direction, float>()
        {
            {Direction.Left, 90 },
            {Direction.LeftUp, 31 },
            {Direction.RightUp, 328 },
            {Direction.Right, 270 },
            {Direction.RightDown, 210 },
            {Direction.LeftDown, 150 }
        };
        
        public static Direction GetDirectionFromAngle(this float angle)
        {
            return RotationToDirection.First(x => Math.Abs(x.Value - angle) < FaultValue).Key;
        }

        public static float GetAngle(this Direction direction)
        {
            return RotationToDirection[direction];
        }
    }
}