using System;
using DG.Tweening;
using Sources.Extensions;
using UnityEngine;

namespace Sources.Core.Roller
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform _rollerRotator;

        [Min(0)] [SerializeField] private float _speed = 1000;

        public void RotateToDirection(Direction direction, Action completed = null)
        {
            float angle = direction.GetAngle();

            _rollerRotator.DOLocalRotate(new Vector3(0, 0, angle), _speed).SetSpeedBased(true).onComplete += delegate
            {
                completed?.Invoke();
            };
        }

        public void ResetRotation()
        {
            _rollerRotator.rotation = Quaternion.identity;
        }
    }
}