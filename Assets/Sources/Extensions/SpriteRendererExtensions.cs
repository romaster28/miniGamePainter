using UnityEngine;

namespace Sources.Extensions
{
    public static class SpriteRendererExtensions
    {
        public static void SetFade(this SpriteRenderer spriteRenderer, float value)
        {
            Color color = spriteRenderer.color;

            color.a = value;

            spriteRenderer.color = color;
        }
    }
}