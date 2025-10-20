using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ViewUIWithCloseButton : ViewUI
    {
        public event Action OnClose;
    
        [SerializeField] Button _closeButton;
    
        private void OnCloseButtonClicked() => OnClose?.Invoke();

        protected override void OnEnable() => _closeButton.onClick.AddListener(OnCloseButtonClicked);

        protected override void OnDisable() => _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }
}