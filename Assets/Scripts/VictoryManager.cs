
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryPanel;

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void CheckVictory(int coins)
    {
        if (LevelLoader.Current != null && LevelLoader.Current.ReportGoal(LevelGoal.CollectCoins, coins))
        {
            if (victoryPanel != null)
                victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
