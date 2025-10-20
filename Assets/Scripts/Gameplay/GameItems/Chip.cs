using Extensions;
using UnityEngine;

namespace Gameplay.GameItems
{
    public class Chip : GameItem
    {
        public Point CurrentPoint
        {
            get => _currentPoint;
            set
            {
                if (_currentPoint != null)
                {
                    _currentPoint.Occupied = false;
                }
            
                _currentPoint = value;
                _currentPoint.Occupied = true;
            }
        }
    
        public int WinPositionPointId { get; private set; }
        public bool ChipInWinPosition => _currentPoint.Id == WinPositionPointId;
    
        [SerializeField] private SpriteRenderer _modelSpriteRenderer;
    
        private Point _currentPoint;


        public void Initialize(int id, GameItemType itemType, Point currentPoint, int winPositionPointId)
        {
            base.Initialize(id, itemType);
        
            CurrentPoint = currentPoint;
            transform.position = CurrentPoint.transform.position;
            WinPositionPointId = winPositionPointId;

            if (_modelSpriteRenderer != null)
            {
                _modelSpriteRenderer.color = id.GetDistinctColor();
            }
        } 
    }
}
