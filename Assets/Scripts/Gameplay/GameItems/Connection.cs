using UnityEngine;

namespace Gameplay.GameItems
{
    public class Connection: GameItem
    {
        public Point PointA { get; private set; }
        public Point PointB { get; private set; }
        
        private LineRenderer Line { get { return _line ??= GetComponent<LineRenderer>(); } }
        private LineRenderer _line;
    
        public void Initialize(int id, GameItemType itemType, Point pointA, Point pointB)
        {
            base.Initialize(id, itemType);
            
            PointA = pointA;
            PointB = pointB;
            
            var middlePosition = (pointA.transform.position + pointB.transform.position) / 2f;
            transform.position = middlePosition;

            Line.positionCount = 2;
            Line.SetPosition(0, pointA.transform.position);
            Line.SetPosition(1, pointB.transform.position);
        }
    }
}