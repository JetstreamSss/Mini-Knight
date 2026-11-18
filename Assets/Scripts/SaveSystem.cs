using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assignment11
{
    [Serializable]
    public sealed class SaveData
    {
        public int version = 1;
        public int bestScore;
        public List<int> unlockedLevels = new() { 1 };
    }

    public static class SaveSystem
    {
        public static string FilePath => Path.Combine(Application.persistentDataPath, "save.json");
        private static string TempPath => Path.Combine(Application.persistentDataPath, "save.tmp");

        public static SaveData Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new SaveData();

                var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
                if (data == null || data.version != 1 || data.bestScore < 0 ||
                    data.unlockedLevels == null || !data.unlockedLevels.Contains(1))
                    throw new InvalidDataException("Invalid progress data.");

                return data;
            }
            catch (Exception error) when (error is IOException || error is InvalidDataException ||
                                          error is UnauthorizedAccessException || error is ArgumentException)
            {
                Debug.LogWarning($"Could not load progress: {error.Message}");
                return new SaveData();
            }
        }

        public static bool Save(SaveData data)
        {
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                File.WriteAllText(TempPath, JsonUtility.ToJson(data, true));
                if (File.Exists(FilePath))
                    File.Replace(TempPath, FilePath, null);
                else
                    File.Move(TempPath, FilePath);

                return true;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException ||
                                          error is NotSupportedException)
            {
                Debug.LogWarning($"Could not save progress: {error.Message}");
                return false;
            }
        }

        public static bool Delete()
        {
            try
            {
                File.Delete(TempPath);
                File.Delete(FilePath);
                return true;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException)
            {
                Debug.LogWarning($"Could not reset progress: {error.Message}");
                return false;
            }
        }
    }
}
