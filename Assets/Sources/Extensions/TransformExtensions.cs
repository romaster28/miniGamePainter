using UnityEngine;

namespace Sources.Extensions
{
    public static class TransformExtensions
    {
        public static void SetPositionZ(this Transform transform, float value)
        {
            Vector3 position = transform.position;

            position.z = value;

            transform.position = position;
        }
    }
}