using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    private int currentHealth;

    private void Awake() => currentHealth = stats.maxHP;

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        MusicManager.Play(GameSound.Hit);
        if (currentHealth > 0)
            return;

        MusicManager.Play(GameSound.Death);
        if (LevelLoader.Current != null && LevelLoader.Current.ReportBossDefeated())
            SceneManager.LoadScene("WinScene");

        Destroy(gameObject);
    }
}
