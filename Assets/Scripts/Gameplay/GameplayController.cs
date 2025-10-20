using System;
using Extensions;
using Gameplay.GameItems;
using LevelLoaderService;
using MiniMap;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameplayController: MonoBehaviour, IGameplayController
    {
        public event Action PlayerWin;
        
        private IBoard _board;
        private IItemSelectionHandler _itemSelectionHandler;
        private IChipMovementHandler _chipMovementHandler;
        private ILevelLoader _levelLoader;
        private IMiniMap _miniMap;
        
        private ISelectLevelPanelUI _selectLevelPanelUI;
        
        private const float _segmentDuration = 0.2f;
        private const float _intervalBtwSegmnt = 0.05f;

        [Inject]
        public void Construct(IBoard board, IItemSelectionHandler itemSelectionHandler,
            IChipMovementHandler chipMovementHandler, ILevelLoader levelLoader,
            IMiniMap miniMap, ISelectLevelPanelUI selectLevelPanelUI)
        {
            _board = board;
            _itemSelectionHandler = itemSelectionHandler;
            _chipMovementHandler = chipMovementHandler;
            _levelLoader = levelLoader;
            _miniMap = miniMap;
            _selectLevelPanelUI = selectLevelPanelUI;
        }

        private void Start()
        {
            var levels = LevelFinder.GetAvailableLevels();
            _selectLevelPanelUI.CreateLevelButtons(levels);
        }

        private void StartSelectedLevel(string levelName)
        {
            var levelPath = LevelFinder.GetLevelFilePath(levelName);
            
            _levelLoader.UnloadCurrentLevel();
            
            if (_levelLoader.TryLoadLevel(levelPath, out var level))
            {
                _board.SetData(level);
                Camera.main.FitToPoints(level.Points);
                
                _miniMap.CreateMiniMapLevel(level);
                _miniMap.SetMiniMapVisible(true);
            }
            
            _itemSelectionHandler.SetClickProcessing(true);
        }

        private void SelectPointToMove(Chip chip, Point targetPoint)
        {
            _itemSelectionHandler.SetClickProcessing(false);
            _chipMovementHandler.MoveChipToPoint(chip, targetPoint, _segmentDuration, _intervalBtwSegmnt);
        }

        private void ChipMovingFinish()
        {
            var levelComplete = _board.CheckWinCondition();

            if (levelComplete)
            {
                PlayerWin?.Invoke();
            }
            else
            {
                _itemSelectionHandler.SetClickProcessing(true);
            }
        }

        private void OnEnable()
        {
            _itemSelectionHandler.OnPointToMoveSelected += SelectPointToMove;
            _chipMovementHandler.OnMovementCompleted += ChipMovingFinish;
            _selectLevelPanelUI.OnLevelSelected += StartSelectedLevel;
        }

        private void OnDisable()
        {
            _selectLevelPanelUI.OnLevelSelected -= StartSelectedLevel;
            _itemSelectionHandler.OnPointToMoveSelected -= SelectPointToMove;
            _chipMovementHandler.OnMovementCompleted -= ChipMovingFinish;
        }
    }
}