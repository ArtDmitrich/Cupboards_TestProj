using System.Collections.Generic;
using Gameplay.GameItems;
using LevelLoaderService;

namespace Gameplay
{
    public interface IBoard
    {
        void SetData(List<Chip> chips, List<Point> points, List<Connection> boardConnections);
        void SetData(LevelData levelData);
        bool CheckWinCondition();
        List<Point> GetAvailableMovePoints(Chip chip);
        List<Point> FindPath(Point startPoint, Point targetPoint);
    }
}