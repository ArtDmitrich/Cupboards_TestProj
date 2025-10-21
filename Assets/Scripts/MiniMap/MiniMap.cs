using CamerasController;
using Extensions;
using FactoryAndPoolObject;
using Gameplay.GameItems;
using LevelLoaderService;
using UI;
using UnityEngine;
using Zenject;
using CameraType = CamerasController.CameraType;

namespace MiniMap
{
    public class MiniMap : MonoBehaviour, IMiniMap
    {
        [SerializeField] private int _miniMapLayer;

        [SerializeField] private Transform _miniMapContainer;
        [SerializeField] private RenderTexture _miniMapRenderTexture;

        private LevelData _miniMapLevelData;

        private IGameItemFactory<Point> _pointFactory;
        private IGameItemFactory<Chip> _chipFactory;
        private IGameItemFactory<Connection> _connectionFactory;
        private IMiniMapUI _miniMapUI;
        private ICamerasController _camerasController;

        [Inject]
        public void Construct(IGameItemFactory<Point> pointFactory, IGameItemFactory<Chip> chipFactory,
            IGameItemFactory<Connection> connectionFactory, IMiniMapUI miniMapUI,
            ICamerasController camerasController)
        {
            _pointFactory = pointFactory;
            _chipFactory = chipFactory;
            _connectionFactory = connectionFactory;
            _miniMapUI = miniMapUI;
            _camerasController = camerasController;
            
            _miniMapLevelData = new LevelData();
            _miniMapUI.SetRenderTexture(_miniMapRenderTexture);
        }

        public void CreateMiniMapLevel(LevelData levelData)
        {
            ClearMiniMap();
            
            CreatePoints(levelData);
            CreateConnections(levelData);
            CreateChipsInWinPositions(levelData);
            
            _camerasController.FitToPointsCamera(CameraType.MiniMapCamera, 
                _miniMapLevelData.GetGameItemTransforms(GameItemType.Point));
            
            gameObject.SetLayerRecursively(_miniMapLayer);
        }

        public void SetMiniMapVisible(bool visible)
        {
            _miniMapContainer.gameObject.SetActive(visible);
            _camerasController.SetCameraEnabled(CameraType.MiniMapCamera, visible);
        }

        private void CreatePoints(LevelData levelData)
        {
            foreach (var point in levelData.Points)
            {
                var miniMapPoint = _pointFactory.Create();
                
                miniMapPoint.Initialize(point.Id, point.ItemType, point.transform.position.x,
                    point.transform.position.y);
                miniMapPoint.Collider.enabled = false;
                miniMapPoint.transform.parent = _miniMapContainer.transform;
                
                _miniMapLevelData.Points.Add(miniMapPoint);
            }
        }

        private void CreateConnections(LevelData levelData)
        {
            if (levelData.Connections == null) return;

            foreach (var connection in levelData.Connections)
            {
                var miniMapPointA = _miniMapLevelData.Points.Find(x => x.Id == connection.PointA.Id);
                var miniMapPointB = _miniMapLevelData.Points.Find(x => x.Id == connection.PointB.Id);

                if (miniMapPointA != null && miniMapPointB != null)
                {
                    var miniMapConnection = _connectionFactory.Create();
                    
                    miniMapConnection.Initialize(connection.Id, GameItemType.Connection, miniMapPointA, miniMapPointB);
                    miniMapConnection.transform.parent = _miniMapContainer.transform;
                    
                    _miniMapLevelData.Connections.Add(miniMapConnection);
                }
            }
        }

        private void CreateChipsInWinPositions(LevelData levelData)
        {
            foreach (var chip in levelData.Chips)
            {
                var targetPoint = _miniMapLevelData.Points.Find(x => x.Id == chip.WinPositionPointId);
                
                if (targetPoint != null)
                {
                    var miniMapChip = _chipFactory.Create();
                    
                    miniMapChip.Initialize(chip.Id, GameItemType.Chip, targetPoint, chip.WinPositionPointId);
                    miniMapChip.Collider.enabled = false;
                    miniMapChip.transform.parent = _miniMapContainer.transform;

                    _miniMapLevelData.Chips.Add(miniMapChip);
                }
            }
        }

        private void ClearMiniMap()
        {
            foreach (var point in _miniMapLevelData.Points)
            {
                point.transform.parent = null;
                _pointFactory.Despawn(point);
            }

            foreach (var chip in _miniMapLevelData.Chips)
            {
                chip.transform.parent = null;
                _chipFactory.Despawn(chip);
            }
            
            foreach (var connection in _miniMapLevelData.Connections)
            {
                connection.transform.parent = null;
                _connectionFactory.Despawn(connection);
            }

            _miniMapLevelData.ClearData();
        }

        private void OnDestroy()
        {
            ClearMiniMap();
        }
    }
}