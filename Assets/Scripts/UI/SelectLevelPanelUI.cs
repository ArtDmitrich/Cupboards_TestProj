using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectLevelPanelUI : ViewUIWithCloseButton, ISelectLevelPanelUI
    {
        public event Action<string> OnLevelSelected;
        
        [SerializeField] private ScrollRect _scrollView;
        [SerializeField] private Transform _contentContainer;
        [SerializeField] private LevelButton _levelButtonPrefab;
        
        private List<LevelButton> _levelButtons = new ();
        
        public void CreateLevelButtons(string[] levels)
        {
            if (levels == null || levels.Length == 0)
            {
                Debug.LogWarning("No levels provided for button creation");
                return;
            }

            for (var i = 0; i < levels.Length; i++)
            {
                CreateLevelButton(levels[i]);
            }
            
            _scrollView.normalizedPosition = Vector2.up;
        }

        private void CreateLevelButton(string levelName)
        {
            var levelButton = Instantiate(_levelButtonPrefab, _contentContainer);
        
            levelButton.SetData(levelName);

            levelButton.OnLevelSelected += OnLevelButtonClicked;
        
            _levelButtons.Add(levelButton);
        }

        private void OnLevelButtonClicked(string levelName)
        {
            OnLevelSelected?.Invoke(levelName);
            Hide();
        }

        private void OnDestroy()
        {
            foreach (var button in _levelButtons)
            {
                button.OnLevelSelected -= OnLevelButtonClicked;
            }
            
            _levelButtons.Clear();
        }
    }
}