using FactoryAndPoolObject;
using Gameplay;
using Gameplay.GameItems;
using Input;
using LevelLoaderService;
using MiniMap;
using UI;
using UnityEngine;
using Zenject;

namespace Bootstrap
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameplayController _gameplayController;
        
        [SerializeField] private ItemSelectionHandler _itemSelectionHandler;
        [SerializeField] private Board _board;
        
        [SerializeField] private Point _pointPrefab;
        [SerializeField] private Chip _chipPrefab;
        [SerializeField] private Connection _connectionPrefab;
        
        [SerializeField] private MiniMap.MiniMap _miniMap;
        
        [SerializeField] private MiniMapUI _miniMapUI;
        [SerializeField] private SelectLevelPanelUI _selectLevelPanelUI;
        
        public override void InstallBindings()
        {
            BindGameplayController();
            BindInputController();
            BindBoard();
            BindChipMovementHandler();
            BindItemSelectionHandler();
            BindPools();
            BindFactories();
            BindLevelLoader();
            BindMiniMap();
            
            BindUI();
        }
        
        private void BindGameplayController()
        {
            Container.Bind<IGameplayController>()
                .FromInstance(_gameplayController)
                .AsSingle();
        }

        private void BindInputController()
        {
            Container.Bind<IInputController>()
                .To<InputController>()
                .AsSingle()
                .NonLazy();
        }

        private void BindBoard()
        {
            Container.Bind<IBoard>()
                .FromInstance(_board)
                .AsSingle();
        }
        
        private void BindChipMovementHandler()
        {
            Container.Bind<IChipMovementHandler>()
                .To<ChipMovementHandler>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindItemSelectionHandler()
        {
            Container.Bind<IItemSelectionHandler>()
                .FromInstance(_itemSelectionHandler)
                .AsSingle();
        }
        
        private void BindPools()
        {
            Container.BindMemoryPool<Point, GameItemPool<Point>>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_pointPrefab)
                .UnderTransformGroup("PointPool");

            Container.BindMemoryPool<Chip, GameItemPool<Chip>>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_chipPrefab)
                .UnderTransformGroup("ChipPool");
            
            Container.BindMemoryPool<Connection, GameItemPool<Connection>>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_connectionPrefab)
                .UnderTransformGroup("ConnectionPool");
        }

        private void BindFactories()
        {
            Container.Bind<IGameItemFactory<Point>>()
                .To<GameItemFactory<Point>>()
                .AsSingle();
            
            Container.Bind<IGameItemFactory<Chip>>()
                .To<GameItemFactory<Chip>>()
                .AsSingle();
            
            Container.Bind<IGameItemFactory<Connection>>()
                .To<GameItemFactory<Connection>>()
                .AsSingle();
        }

        private void BindLevelLoader()
        {
            Container.Bind<ILevelLoader>()
                .To<LevelLoader>()
                .AsSingle()
                .NonLazy();
        }

        private void BindMiniMap()
        {
            Container.Bind<IMiniMap>()
                .FromInstance(_miniMap)
                .AsSingle();
        }
        
        private void BindUI()
        {
            Container.Bind<IMiniMapUI>()
                .FromInstance(_miniMapUI)
                .AsSingle();
            
            Container.Bind<ISelectLevelPanelUI>()
                .FromInstance(_selectLevelPanelUI)
                .AsSingle();
        }
    }
}