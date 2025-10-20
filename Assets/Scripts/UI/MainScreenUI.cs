using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class MainScreenUI : MonoBehaviour
    {
        [SerializeField] private Button _miniMapButton;
        [SerializeField] private Button _selectLevelPanelButton;

        [SerializeField] private ViewUIWithCloseButton _miniMap;
        [SerializeField] private ViewUIWithCloseButton _selectLevelPanel;
        [SerializeField] private ViewUIWithCloseButton _winScreen;
        
        private IGameplayController _gameplayController;
        
        [Inject]
        public void Construct(IGameplayController gameplayController) 
            => _gameplayController = gameplayController;
        
        private void OpenMiniMap() => _miniMap.gameObject.SetActive(true);
        
        private void CloseMiniMap() => _miniMap.gameObject.SetActive(false);
        
        private void OpenSelectLevelPanel() => _selectLevelPanel.gameObject.SetActive(true);
        
        private void CloseSelectLevelPanel() => _selectLevelPanel.gameObject.SetActive(false);
        
        private void OpenWinScreen() => _winScreen.gameObject.SetActive(true);
        private void CloseWinScreen() => _winScreen.gameObject.SetActive(false);
        
        private void OnEnable()
        {
            _miniMapButton.onClick.AddListener(OpenMiniMap);
            _selectLevelPanelButton.onClick.AddListener(OpenSelectLevelPanel);
            
            _miniMap.OnClose += CloseMiniMap;
            _selectLevelPanel.OnClose += CloseSelectLevelPanel;
            
            _gameplayController.PlayerWin += OpenWinScreen;
            _winScreen.OnClose += CloseWinScreen;

        }

        private void OnDisable()
        {
            _miniMapButton.onClick.RemoveListener(OpenMiniMap);
            _selectLevelPanelButton.onClick.RemoveListener(OpenSelectLevelPanel);
            
            _miniMap.OnClose -= CloseMiniMap;
            _selectLevelPanel.OnClose -= CloseSelectLevelPanel;
            
            _gameplayController.PlayerWin -= OpenWinScreen;
            _winScreen.OnClose -= CloseWinScreen;
        }
    }
}