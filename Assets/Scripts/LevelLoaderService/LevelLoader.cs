using System;
using System.IO;
using System.Linq;
using FactoryAndPoolObject;
using Gameplay.GameItems;
using UnityEngine;
using Zenject;

namespace LevelLoaderService
{
    public class LevelLoader: ILevelLoader
    {
        private IGameItemFactory<Point> _pointFactory;
        private IGameItemFactory<Chip> _chipFactory;
        private IGameItemFactory<Connection> _connectionFactory;

        private string _currentFilePath;
        private readonly LevelData _currentLevelData;
        
        [Inject]
        public LevelLoader (IGameItemFactory<Point> pointFactory, IGameItemFactory<Chip> chipFactory,
            IGameItemFactory<Connection> connectionFactory)
        {
            _pointFactory = pointFactory;
            _chipFactory = chipFactory;
            _connectionFactory = connectionFactory;
            
            _currentLevelData = new LevelData();
        }
        
        public bool TryLoadLevel(string filePath, out LevelData levelData)
        {
            levelData = _currentLevelData;
            
            if (!File.Exists(filePath))
            {
                Debug.LogError("File not found: " + filePath);
                return false;
            }

            var lines = File.ReadAllLines(filePath);

            try
            {
                var chipCount = int.Parse(lines[0]);
                var pointCount = int.Parse(lines[1]);
                
                var currentGameItemId = 0;
                
                for (var i = 0; i < pointCount; i++)
                {
                    var coords = lines[2 + i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var x = float.Parse(coords[0]);
                    var y = float.Parse(coords[1]);

                    var newPoint = _pointFactory.Create();
                    
                    currentGameItemId++;
                    newPoint.Initialize(currentGameItemId, GameItemType.Point, x, y);
                    
                    _currentLevelData.Points.Add(newPoint);
                }

                var startPos = lines[2 + pointCount].Split(",", StringSplitOptions.RemoveEmptyEntries);
                var startPositionsPointId = new int[chipCount];
                for (var i = 0; i < chipCount; i++)
                {
                    startPositionsPointId[i] = int.Parse(startPos[i]);
                }

                var winPos = lines[3 + pointCount].Split(",", StringSplitOptions.RemoveEmptyEntries);
                var winPositionsPointId = new int[chipCount];
                for (var i = 0; i < chipCount; i++)
                {
                    winPositionsPointId[i] = int.Parse(winPos[i]);
                }
                
                for (var i = 0; i < chipCount; i++)
                {
                    var newChip = _chipFactory.Create();
                    
                    currentGameItemId++;
                    var startPoint = _currentLevelData.Points.FirstOrDefault(p => p.Id == startPositionsPointId[i]); 
                    newChip.Initialize(currentGameItemId, GameItemType.Chip, startPoint, winPositionsPointId[i]);
                    
                    _currentLevelData.Chips.Add(newChip);
                }

                var connectionCount = int.Parse(lines[4 + pointCount]);

                for (var i = 0; i < connectionCount; i++)
                {
                    var conn = lines[5 + pointCount + i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var a = int.Parse(conn[0]);
                    var b = int.Parse(conn[1]);
                    
                    var newConnection = _connectionFactory.Create();
                    
                    currentGameItemId++;
                    var pointA = _currentLevelData.Points.FirstOrDefault(p => p.Id == a);
                    var pointB = _currentLevelData.Points.FirstOrDefault(p => p.Id == b);
                    newConnection.Initialize(currentGameItemId, GameItemType.Connection, pointA, pointB);
                    
                    _currentLevelData.Connections.Add(newConnection);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error parsing level file: " + e.Message);
                return false;
            }
            
            return true;
        }
        
        public void UnloadCurrentLevel()
        {
            foreach (var point in _currentLevelData.Points)
            {
                _pointFactory.Despawn(point);
            }

            foreach (var chip in _currentLevelData.Chips)
            {
                _chipFactory.Despawn(chip);
            }

            foreach (var connection in _currentLevelData.Connections)
            {
                _connectionFactory.Despawn(connection);
            }

            _currentLevelData.ClearData();
        }
    }
}