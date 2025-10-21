using System.Collections.Generic;
using Gameplay.GameItems;
using UnityEngine;

namespace LevelLoaderService
{
    [System.Serializable]
    public class LevelData
    {
        public List<Point> Points = new();
        public List<Connection> Connections = new();
        public List<Chip> Chips = new();

        public void ClearData()
        {
            Points.Clear();
            Connections.Clear();
            Chips.Clear();
        }

        public List<Transform> GetGameItemTransforms(GameItemType gameItemType)
        {
            var result = new List<Transform>();
            var targetGameItems = new List<GameItem>();

            switch (gameItemType)
            {
                case GameItemType.Point:
                    targetGameItems.AddRange(Points);
                    break;
                
                case GameItemType.Chip:
                    targetGameItems.AddRange(Chips);
                    break;
                
                case GameItemType.Connection:
                    targetGameItems.AddRange(Connections);
                    break;
                default:
                    break;
            }

            foreach (var gameItem in targetGameItems)
            {
                if (gameItem != null && gameItem.transform != null)
                {
                    result.Add(gameItem.transform);
                }
            }

            return result;
        }
        
    }
}