
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coins = 0;
    public TMP_Text coinText;
    public VictoryManager victoryManager;
    public AudioSource coinAudioSource;

    private void Awake()
    {
        Instance = this;
        UpdateText();
    }

    public void AddCoin()
    {
        coins++;
        UpdateText();

        if (coinAudioSource != null)
        {
            coinAudioSource.PlayOneShot(coinAudioSource.clip);
        }

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
