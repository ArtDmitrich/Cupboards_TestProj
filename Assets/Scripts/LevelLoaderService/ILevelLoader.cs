namespace LevelLoaderService
{
    public interface ILevelLoader
    {
        bool TryLoadLevel(string filePath, out LevelData levelData);
        void UnloadCurrentLevel();
    }
}