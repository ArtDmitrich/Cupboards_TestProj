using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : ViewUI
    {
        public event Action<string> OnLevelSelected;
        
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        
        public void SetData(string levelName) => _text.text = levelName;
        
        private void OnClick() => OnLevelSelected?.Invoke(_text.text);

        protected override void OnEnable() => _button.onClick.AddListener(OnClick);

        protected override void OnDisable() => _button.onClick.RemoveListener(OnClick);
    }
}