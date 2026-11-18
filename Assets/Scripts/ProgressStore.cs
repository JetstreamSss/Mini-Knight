using Assignment11;
using UnityEngine;

public sealed class ProgressStore : MonoBehaviour
{
    public const string MusicVolumeKey = "miniknight.musicVolume";
    public const string VibrationKey = "miniknight.vibration";
    public static SaveData Data { get; private set; }
    public static bool VibrationEnabled => PlayerPrefs.GetInt(VibrationKey, 1) != 0;

    private static bool needsSave;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Data = SaveSystem.Load();
        needsSave = false;
        var obj = new GameObject("Mini-Knight Progress", typeof(ProgressStore));
        DontDestroyOnLoad(obj);
    }

    public static void Complete(LevelData level)
    {
        if (!Data.unlockedLevels.Contains(level.number))
            return;

        Data.bestScore = Mathf.Max(level.completionScore, Data.bestScore);
        if (LevelData.Find(level.number + 1) != null && !Data.unlockedLevels.Contains(level.number + 1))
            Data.unlockedLevels.Add(level.number + 1);

        needsSave = !SaveSystem.Save(Data);
    }

    public static bool ResetProgress()
    {
        if (!SaveSystem.Delete())
            return false;

        Data = new SaveData();
        needsSave = false;
        PlayerPrefs.DeleteKey(MusicVolumeKey);
        PlayerPrefs.DeleteKey(VibrationKey);
        PlayerPrefs.Save();
        return true;
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused && needsSave)
            needsSave = !SaveSystem.Save(Data);
    }
}
