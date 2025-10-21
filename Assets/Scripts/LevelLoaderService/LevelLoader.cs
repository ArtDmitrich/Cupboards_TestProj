using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using FactoryAndPoolObject;
using Gameplay.GameItems;
using LoggerService;
using UnityEngine;
using Zenject;

namespace LevelLoaderService
{
    public class LevelLoader: ILevelLoader
    {
        private IGameItemFactory<Point> _pointFactory;
        private IGameItemFactory<Chip> _chipFactory;
        private IGameItemFactory<Connection> _connectionFactory;
        private ILoggerService _loggerService;

        private string _currentFilePath;
        private readonly LevelData _currentLevelData;
        
        private const int GAME_ITEMS_LAYER_MASK_INDEX = 6;
        
        [Inject]
        public LevelLoader (IGameItemFactory<Point> pointFactory, IGameItemFactory<Chip> chipFactory,
            IGameItemFactory<Connection> connectionFactory, ILoggerService loggerService)
        {
            _pointFactory = pointFactory;
            _chipFactory = chipFactory;
            _connectionFactory = connectionFactory;
            _loggerService = loggerService;
            
            _currentLevelData = new LevelData();
        }
        
        public bool TryLoadLevel(string filePath, out LevelData levelData)
        {
            levelData = _currentLevelData;
            _currentLevelData.ClearData();
            
            if (ValidateBasicFileStructure(filePath, out var lines))
            {
                return CreateLevelObjects(lines, out levelData);
            }

            return false;
        }

        private bool ValidateBasicFileStructure(string filePath, out string[] lines)
        {
            if (!File.Exists(filePath))
            {
                _loggerService.LogError("File not found: " + filePath);
                lines = Array.Empty<string>();
                return false;
            }

            lines = File.ReadAllLines(filePath);

            // Проверяем минимальное количество строк
            if (lines.Length < 5)
            {
                _loggerService.LogError($"File too short. Expected at least 5 lines, got {lines.Length}");
                return false;
            }

            // 1. Количество фишек
            if (!int.TryParse(lines[0], out var chipCount) || chipCount <= 0)
            {
                _loggerService.LogError($"Invalid chip count: {lines[0]}");
                return false;
            }

            // 2. Количество точек
            if (!int.TryParse(lines[1], out var pointCount) || pointCount <= 0)
            {
                _loggerService.LogError($"Invalid point count: {lines[1]}");
                return false;
            }

            // Проверяем, что есть достаточно строк для точек
            if (lines.Length < 2 + pointCount)
            {
                _loggerService.LogError($"Not enough lines for points. Need {2 + pointCount}, got {lines.Length}");
                return false;
            }

            // 14. Количество соединений
            var connectionCountLineIndex = 4 + pointCount;
            
            if (connectionCountLineIndex >= lines.Length)
            {
                _loggerService.LogError($"Missing connection count at line {connectionCountLineIndex}");
                return false;
            }

            if (!int.TryParse(lines[connectionCountLineIndex], out var connectionCount) || connectionCount < 0)
            {
                _loggerService.LogError($"Invalid connection count: {lines[connectionCountLineIndex]}");
                return false;
            }

            // Проверяем общее количество строк
            var expectedTotalLines = 5 + pointCount + connectionCount;
            
            if (lines.Length < expectedTotalLines)
            {
                _loggerService.LogError($"Not enough lines. Expected {expectedTotalLines}, got {lines.Length}");
                return false;
            }

            return true;
        }
        
        private bool CreateLevelObjects(string[] lines, out LevelData levelData)
        {
            levelData = _currentLevelData;

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
                    newPoint.Collider.enabled = true;
                    newPoint.gameObject.SetLayerRecursively(GAME_ITEMS_LAYER_MASK_INDEX);
                    
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
                    newChip.Collider.enabled = true;
                    newChip.gameObject.SetLayerRecursively(GAME_ITEMS_LAYER_MASK_INDEX);
                    
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
                    newConnection.gameObject.SetLayerRecursively(GAME_ITEMS_LAYER_MASK_INDEX);
                    
                    _currentLevelData.Connections.Add(newConnection);
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError("Error parsing level file: " + e.Message);
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