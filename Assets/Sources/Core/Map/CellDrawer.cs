using System;
using System.Collections.Generic;
using DG.Tweening;
using Sources.Extensions;
using UnityEngine;

namespace Sources.Core.Map
{
    public class CellDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject _coloredPrefab;

        [SerializeField] private Transform _parent;

        [Min(0)] [SerializeField] private float _fadeDurationStart = .5f;

        private readonly List<Cell> _paintedCells = new List<Cell>();

        private readonly List<GameObject> _paints = new List<GameObject>();
        
        public bool IsPainted(Cell cell) => _paintedCells.Contains(cell);

        public void DrawOnCell(Cell cell)
        {
            if (IsPainted(cell))
                throw new ArgumentException($"Can't draw on {cell.name}, because it's already painted");

            GameObject created = Instantiate(_coloredPrefab, cell.MiddlePoint, Quaternion.identity, _parent);

            created.transform.SetPositionZ(created.transform.position.y);

            var spriteRenderer = created.GetComponent<SpriteRenderer>();
            
            spriteRenderer.SetFade(0);
            
            spriteRenderer.DOFade(1, _fadeDurationStart);
            
            _paints.Add(created);

            _paintedCells.Add(cell);
        }

        public void ClearAll()
        {
            foreach (var paint in _paints)
            {
                Destroy(paint.gameObject);
            }
            
            _paintedCells.Clear();
            
            _paints.Clear();
        }
    }
}