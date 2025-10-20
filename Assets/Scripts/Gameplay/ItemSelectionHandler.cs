using System;
using System.Collections.Generic;
using Gameplay.GameItems;
using Input;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class ItemSelectionHandler : MonoBehaviour, IItemSelectionHandler
    {
        public event Action<Chip, Point> OnPointToMoveSelected;

        private IInputController _inputController;
        private IBoard _board;

        private Chip _selectedChip;
        private readonly List<Point> _selectedPoints = new();

        private Camera _mainCamera;
        private bool  _isClickProcessingEnable;
        
        [Inject]
        public void Construct(IBoard board, IInputController inputController)
        {
            _board = board;
            _inputController = inputController;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void SetClickProcessing(bool enable) => _isClickProcessingEnable = enable;
        
        private void HandleClick(Vector2 clickPosition)
        {
            if (!_isClickProcessingEnable)
            {
                return;
            }
            
            var mousePos = _mainCamera.ScreenToWorldPoint(clickPosition);
            var hit = Physics2D.Raycast(mousePos, Vector2.zero,
                Mathf.Infinity, 1 << LayerMask.NameToLayer("GameItems"));

            if (hit.collider != null)
            {
                var clickedItem = hit.collider.GetComponent<GameItem>();

                if (clickedItem != null)
                {
                    switch (clickedItem.ItemType)
                    {
                        case GameItemType.Chip:
                            SelectItem(clickedItem as Chip);
                            break;
                        case GameItemType.Point:
                            SelectItem(clickedItem as Point);
                            break;
                        case GameItemType.Connection:
                        default:
                            break;
                    }
                    
                    return;
                }
            }

            UnselectAllGameItems();
        }

        private void SelectItem(Chip chip)
        {
            if (_selectedChip != null && _selectedChip != chip)
            {
                UnselectAllGameItems();
            }

            _selectedChip = chip;
            _selectedChip?.Select();

            HighlightAvailablePoints();
        }

        private void SelectItem(Point point)
        {
            if (_selectedChip != null && _selectedPoints.Contains(point))
            {
                OnPointToMoveSelected?.Invoke(_selectedChip, point);

                UnselectAllGameItems();
            }
        }

        private void UnselectAllGameItems()
        {
            if (_selectedChip != null)
            {
                _selectedChip.Unselect();
                _selectedChip = null;
            }

            foreach (var point in _selectedPoints)
            {
                point.Unselect();
            }

            _selectedPoints.Clear();
        }

        private void HighlightAvailablePoints()
        {
            var points = _board.GetAvailableMovePoints(_selectedChip);

            foreach (var point in points)
            {
                point.Select();
                _selectedPoints.Add(point);
            }
        }

        private void OnEnable()
        {
            _inputController.OnLeftMouseButtonClicked += HandleClick;
        }

        private void OnDisable()
        {
            _inputController.OnLeftMouseButtonClicked -= HandleClick;
        }
    }
}