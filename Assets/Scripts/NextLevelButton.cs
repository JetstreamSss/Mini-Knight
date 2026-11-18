using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        var current = LevelData.Find(SceneManager.GetActiveScene().name);
        var next = current == null ? null : LevelData.Find(current.number + 1);
        if (next != null && ProgressStore.Data.unlockedLevels.Contains(next.number))
            next.Play();
    }
}
