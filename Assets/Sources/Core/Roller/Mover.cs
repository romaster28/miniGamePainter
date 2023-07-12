using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Sources.Core.Roller
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform _roller;

        [Min(0)] [SerializeField] private float _speed = 3;

        public Vector2 CurrentPosition => _roller.position;
        
        public void SetPosition(Vector2 position)
        {
            _roller.position = position;
        }

        public async void MoveOnPathAsync(IEnumerable<Vector2> steps, Action completed, Action<Vector2> stepCompleted = null)
        {
            foreach (var step in steps)
            {
                await _roller.DOMove(step, _speed).SetSpeedBased(true).SetEase(Ease.Linear).AsyncWaitForCompletion();
                
                stepCompleted?.Invoke(step);
            }
            
            completed?.Invoke();
        }
    }
}