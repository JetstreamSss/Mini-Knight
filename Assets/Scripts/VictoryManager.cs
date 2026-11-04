
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryPanel;
    public int targetCoins = 10;

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void CheckVictory(int coins)
    {
        if (coins >= targetCoins && victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
