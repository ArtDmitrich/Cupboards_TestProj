using LevelLoaderService;

namespace MiniMap
{
    public interface IMiniMap
    {
        void CreateMiniMapLevel(LevelData levelData);
        void SetMiniMapVisible(bool visible);
    }
}