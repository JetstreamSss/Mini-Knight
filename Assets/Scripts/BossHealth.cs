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
        if (currentHealth > 0)
            return;

        if (LevelLoader.Current != null && LevelLoader.Current.ReportBossDefeated())
            SceneManager.LoadScene("WinScene");

        Destroy(gameObject);
    }
}
