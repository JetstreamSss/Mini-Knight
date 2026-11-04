
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        string scene = SceneManager.GetActiveScene().name.ToLower();

        if (scene == "samplescene")
        {
            SceneManager.LoadScene("Level2");
        }
        else if (scene == "level2")
        {
            SceneManager.LoadScene("Level3");
        }
        else
        {
            Debug.LogError("Неизвестный уровень: " + scene);
        }
    }
}
