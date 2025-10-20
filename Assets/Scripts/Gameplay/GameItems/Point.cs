using UnityEngine;

namespace Gameplay.GameItems
{
    public class Point : GameItem
    {
        public bool Occupied
        {
            get => _occupied;
            set
            {
                _occupied = value;
                Collider.enabled = !_occupied;
            }
        }
    
        private bool _occupied;

        public void Initialize(int id, GameItemType itemType, float positionX, float positionY)
        {
            base.Initialize(id, itemType);
        
            transform.position = new Vector2(positionX, positionY);
            Occupied = false;
        }
    }
}
