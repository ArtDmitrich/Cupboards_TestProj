using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.GameItems;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class ChipMovementHandler:  IChipMovementHandler, IDisposable
    {
        public event Action OnMovementCompleted;
        
        private readonly IBoard _board;
        
        private Sequence _movementSequence;
        
        [Inject]
        public ChipMovementHandler(IBoard board)
        {
            _board = board;
        }

        private bool IsMoving => _movementSequence != null && _movementSequence.IsPlaying();
        
        public void MoveChipToPoint(Chip chip, Point targetPoint, float segmentDuration, float intervalBtwSegment)
        {
            if (IsMoving)
            {
                return;
            }
            
            if (chip == null || targetPoint == null)
            {
                return;
            }
            
            var path = _board.FindPath(chip.CurrentPoint, targetPoint);
            
            if (path == null || path.Count < 2)
            {
                Debug.LogWarning($"No valid path found from point {chip.CurrentPoint.Id} to point {targetPoint.Id}");
                return;
            }
            
            StartMovementSequence(chip, path, segmentDuration, intervalBtwSegment);
        }
        
        private void StartMovementSequence(Chip chip, List<Point> path, float segmentDuration, float intervalBtwSegment)
        {
            _movementSequence?.Kill();
            _movementSequence = DOTween.Sequence();
            
            for (var i = 1; i < path.Count; i++)
            {
                var targetPoint = path[i];
                
                _movementSequence.AppendCallback(() =>
                {
                    if (chip.CurrentPoint != null)
                    {
                        chip.CurrentPoint.Occupied = false;
                    }
                });
                
                _movementSequence.Append(chip.transform
                    .DOMove(targetPoint.transform.position, segmentDuration)
                    .SetEase(Ease.OutQuad));
                
                _movementSequence.AppendCallback(() => 
                {
                    chip.CurrentPoint = targetPoint;
                    
                    if (targetPoint == path[^1])
                    {
                        OnMovementCompleted?.Invoke();
                    }
                });
                
                if (i < path.Count - 1)
                {
                    _movementSequence.AppendInterval(intervalBtwSegment);
                }
            }
            
            _movementSequence.OnComplete(() => { _movementSequence = null; });
            
            _movementSequence.OnKill(() => { _movementSequence = null; });
            
            _movementSequence.Play();
        }

        public void Dispose()
        {
            _movementSequence?.Kill();
        }
    }
}