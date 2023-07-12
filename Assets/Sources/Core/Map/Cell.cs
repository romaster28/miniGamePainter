using UnityEngine;

namespace Sources.Core.Map
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private bool _blockedOnStart;

        public bool BlockedOnStart => _blockedOnStart;

        public Vector2 MiddlePoint => transform.position;
    }
}