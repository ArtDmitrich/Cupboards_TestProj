using System.IO;
using System.Linq;
using UnityEngine;

namespace LevelLoaderService
{
    public static class LevelFinder
    {
        public static string GetLevelFilePath(string fileName)
        {
            return Path.Combine(GetLevelsFolderPath(), fileName);
        }

        public static string[] GetAvailableLevels()
        {
            var levelsFolder = GetLevelsFolderPath();

            if (!Directory.Exists(levelsFolder))
            {
                return null;
            }

            return Directory.GetFiles(levelsFolder, "*.txt")
                .Select(Path.GetFileName)
                .ToArray();
        }
        
        private static string GetLevelsFolderPath()
        {
#if UNITY_EDITOR
            // В редакторе: Assets/Resources/Levels/
            return Path.Combine(Application.dataPath, "Resources", "Levels");
#else
            // В билде: рядом с exe/Levels/
            string exePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            return Path.Combine(exePath, "Levels");
#endif
        }
    }
}