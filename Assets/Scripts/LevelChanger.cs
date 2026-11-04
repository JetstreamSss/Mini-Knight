
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelChanger : MonoBehaviour
{
    public GameObject loadingScreen;

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Level2");
    }
}
