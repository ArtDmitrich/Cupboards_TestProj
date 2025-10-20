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
            // В билде: рядом с exe файлом/Levels/
            string exeDirectory = GetExecutableDirectory();
            return Path.Combine(exeDirectory, "Levels");
#endif
        }

        private static string GetExecutableDirectory()
        {
#if UNITY_STANDALONE_WIN
            // Для Windows билда
            return Directory.GetParent(Application.dataPath).FullName;
#elif UNITY_STANDALONE_OSX
            // Для Mac билда
            return Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
#elif UNITY_STANDALONE_LINUX
            // Для Linux билда
            return Directory.GetParent(Application.dataPath).FullName;
#else
            // Для других платформ (мобильные, веб и т.д.)
            return Application.persistentDataPath;
#endif
        }
    }
}