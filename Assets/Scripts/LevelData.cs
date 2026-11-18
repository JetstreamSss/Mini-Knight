using UnityEngine;

public enum LevelGoal { CollectCoins, DefeatBoss }

[CreateAssetMenu(fileName = "Level_01", menuName = "Game/Level Data")]
public sealed class LevelData : ScriptableObject
{
    [System.Serializable]
    public sealed class Wave
    {
        public GameObject enemyPrefab;
        [Min(0)] public float delay;
        public Vector2[] spawnPoints;
    }

    [Min(1)] public int number;
    public string sceneName;
    [Min(1)] public float timeLimit;
    public LevelGoal goal;
    [Min(1)] public int goalAmount;
    [Min(0)] public int completionScore;
    public Wave[] waves;

    private static LevelData[] levels;
    public static LevelData[] All => levels ??= Resources.LoadAll<LevelData>("Levels");

    public static LevelData Find(int number) => System.Array.Find(All, level => level.number == number);
    public static LevelData Find(string scene) => System.Array.Find(All, level => level.sceneName == scene);

    public void Play()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
