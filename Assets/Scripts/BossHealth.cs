
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;

    private int currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log("Босс получил урон! Осталось HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Босс побеждён!");

            string scene = SceneManager.GetActiveScene().name.ToLower();
            Debug.Log("Текущая сцена: " + scene);

            if (scene == "level3")
            {
                Debug.Log("Загружаю WinScene!");
                Time.timeScale = 1f;
                SceneManager.LoadScene("WinScene");
                return;
            }

            Destroy(gameObject);
        }
    }
}
