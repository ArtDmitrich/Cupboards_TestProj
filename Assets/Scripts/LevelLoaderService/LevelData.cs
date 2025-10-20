using System.Collections.Generic;
using Gameplay.GameItems;

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
    }
}