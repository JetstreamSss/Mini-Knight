
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coins = 0;
    public TMP_Text coinText;
    public VictoryManager victoryManager;

    private void Awake()
    {
        Instance = this;
        UpdateText();
    }

    public void AddCoin()
    {
        coins++;
        UpdateText();

        MusicManager.Play(GameSound.Pickup);

        if (victoryManager != null)
        {
            victoryManager.CheckVictory(coins);
        }
    }

    private void UpdateText()
    {
        if (coinText != null)
        {
            coinText.text = "Монеты: " + coins;
        }
    }
}
