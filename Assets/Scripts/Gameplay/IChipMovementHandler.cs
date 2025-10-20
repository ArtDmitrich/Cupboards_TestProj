using System;
using Gameplay.GameItems;

namespace Gameplay
{
    public interface IChipMovementHandler
    {
        event Action OnMovementCompleted;
        void MoveChipToPoint(Chip chip, Point targetPoint, float segmentDuration, float intervalBtwSegment);
    }
}