using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Core.Map;
using Sources.Extensions;
using UnityEngine;

namespace Sources.Core.UserControl
{
    public class DirectionChooser : MonoBehaviour
    {
        [SerializeField] private Arrow[] _arrows;

        public event Action<Direction> Chosen;

        public int ShowedCount => _arrows.Count(x => x.IsShown);
        
        public void Initialize()
        {
            foreach (var arrow in _arrows)
            {
                arrow.Entered += delegate { OnMouseEnteredArrow(arrow); };

                arrow.Exit += delegate { OnMouseExitArrow(arrow); };

                arrow.Clicked += delegate { OnMouseClickedArrow(arrow); };
            }
        }

        public void ShowFor(Vector2 middlePoint, IEnumerable<Cell> nearCells)
        {
            if (nearCells.Count() > _arrows.Length)
                throw new ArgumentException($"There is too many near cells");

            foreach (var nearCell in nearCells)
            {
                Arrow arrow = GetAndShowFreeArrow();

                Transform arrowTransform = arrow.transform;

                arrowTransform.position = nearCell.MiddlePoint;

                Vector3 relative = arrowTransform.InverseTransformPoint(middlePoint);

                float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg + 180;

                arrowTransform.Rotate(0, 0, -angle);
            }
        }

        public void HideAll()
        {
            foreach (var arrow in _arrows)
                arrow.Hide();
        }

        private Arrow GetAndShowFreeArrow()
        {
            Arrow arrow = _arrows.First(x => !x.IsShown);

            arrow.Show();

            return arrow;
        }

        private void OnMouseEnteredArrow(Arrow arrow)
        {
            arrow.SetChosen(true);
        }

        private void OnMouseExitArrow(Arrow arrow)
        {
            arrow.SetChosen(false);
        }

        private void OnMouseClickedArrow(Arrow arrow)
        {
            float angle = arrow.transform.localRotation.eulerAngles.z;

            Chosen?.Invoke(angle.GetDirectionFromAngle());
        }
    }
}