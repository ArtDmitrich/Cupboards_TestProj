using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.GameItems;
using LevelLoaderService;
using UnityEngine;

namespace Gameplay
{
    public class Board: MonoBehaviour, IBoard
    {
        private  List<Chip> _chips = new ();
        private  List<Connection> _connections = new ();
        private  Dictionary<int, Point> _points = new ();
    
        public void SetData(List<Chip> chips, List<Point> points, List<Connection> boardConnections)
        {
            _chips = chips;
            _connections = boardConnections;
            
            foreach (var point in points)
            {
                _points[point.Id] = point;
            }
        }
        
        public void SetData(LevelData levelData)
        {
            _chips = levelData.Chips;
            _connections = levelData.Connections;
            
            foreach (var point in levelData.Points)
            {
                _points[point.Id] = point;
            }
        }
    
        public bool CheckWinCondition() => _chips.All(t => t.ChipInWinPosition);
        
        public List<Point> GetAvailableMovePoints(Chip chip)
        {
            var availablePoints = new List<Point>();
            var currentPoint = chip.CurrentPoint;
            
            foreach (var point in _points.Values)
            {
                if (point != currentPoint && !point.Occupied)
                {
                    var path = FindPath(currentPoint, point);
                    
                    if (path != null && path.Count > 1)
                    {
                        availablePoints.Add(point);
                    }
                }
            }
            
            return availablePoints;
        }
        
        public List<Point> FindPath(Point startPoint, Point targetPoint)
        {
            try
            {
                var cameFrom = new Dictionary<Point, Point>();
                var queue = new Queue<Point>();
                var visited = new HashSet<Point>();
                
                queue.Enqueue(startPoint);
                visited.Add(startPoint);
                cameFrom[startPoint] = null;
                
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    
                    if (current == targetPoint)
                    {
                        return ReconstructPath(cameFrom, targetPoint);
                    }
                    
                    var neighbors = GetNeighbors(current);
                    
                    foreach (var neighbor in neighbors)
                    {
                        if (visited.Contains(neighbor) || (neighbor.Occupied && neighbor != targetPoint))
                        {
                            continue;
                        }
                        
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error find path: " + e.Message);
            }
            
            return null;
        }
        
        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point targetPoint)
        {
            var path = new List<Point>();
            var current = targetPoint;
            
            while (current != null)
            {
                path.Add(current);
                current = cameFrom[current];
            }
            
            path.Reverse();
            return path;
        }
        
        private List<Point> GetNeighbors(Point point)
        {
            var neighbors = new List<Point>();
        
            foreach (var connection in _connections)
            {
                if (connection.PointA == point)
                {
                    var neighbor = _points[connection.PointB.Id];

                    if (neighbor != null)
                    {
                        neighbors.Add(neighbor);
                    }
                }
                else if (connection.PointB == point)
                {
                    var neighbor = _points[connection.PointA.Id];

                    if (neighbor != null)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
        
            return neighbors;
        }
    }
}