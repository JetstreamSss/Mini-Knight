
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invincibilityDuration = 1f;
    public float blinkInterval = 0.1f;
    public float respawnDelay = 2f;
    public Sprite deathSprite;

    public TMP_Text healthText;
    public AudioSource damageAudio;

    private SpriteRenderer mainSpriteRenderer;
    private Sprite originalSprite;
    private Animator playerAnimator;

    private int currentHealth;
    private bool isInvincible;
    private bool isDead;

    private Vector3 spawnPosition;
    private Rigidbody2D rb;
    private PlayerController playerController;

    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    private float originalGravityScale;

    private void Awake()
    {
        spawnPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();

        if (rb != null)
            originalGravityScale = rb.gravityScale;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[spriteRenderers.Length];

        mainSpriteRenderer = GetComponent<SpriteRenderer>();

        if (mainSpriteRenderer == null)
            mainSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (mainSpriteRenderer != null)
            originalSprite = mainSpriteRenderer.sprite;

        for (int i = 0; i < spriteRenderers.Length; i++)
            originalColors[i] = spriteRenderers[i].color;

        if (damageAudio == null)
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();

            if (audioSources.Length > 1)
                damageAudio = audioSources[1];
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        Debug.Log("HP рыцаря: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || isDead || currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthText();

        if (damageAudio != null && damageAudio.clip != null)
            damageAudio.PlayOneShot(damageAudio.clip);

        Debug.Log("HP рыцаря: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(Respawn());
            return;
        }

        StartCoroutine(DamageEffect());
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = "ХП: " + currentHealth + " / " + maxHealth;
    }

    private IEnumerator DamageEffect()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            SetAlpha(0.15f);
            yield return new WaitForSeconds(blinkInterval);

            SetAlpha(1f);
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2f;
        }

        SetAlpha(1f);
        isInvincible = false;
    }

    private IEnumerator Respawn()
    {
        isDead = true;

        if (playerController != null)
            playerController.enabled = false;

        if (playerAnimator != null)
            playerAnimator.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = originalGravityScale;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (mainSpriteRenderer != null && deathSprite != null)
        {
            mainSpriteRenderer.sprite = deathSprite;
            SetAlpha(1f);
        }
        else
        {
            SetAlpha(0f);
            Debug.LogWarning("Назначь картинку в поле Death Sprite!");
        }

        Debug.Log("Рыцарь погиб!");

        yield return new WaitForSeconds(respawnDelay);

        transform.position = spawnPosition;

        if (mainSpriteRenderer != null)
            mainSpriteRenderer.sprite = originalSprite;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = originalGravityScale;
        }

        currentHealth = maxHealth;
        UpdateHealthText();
        SetAlpha(1f);

        if (playerAnimator != null)
            playerAnimator.enabled = true;

        isInvincible = false;
        isDead = false;

        if (playerController != null)
            playerController.enabled = true;

        Debug.Log("Рыцарь возродился! HP: " + currentHealth);
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] == null)
                continue;

            Color color = originalColors[i];
            color.a = alpha;
            spriteRenderers[i].color = color;
        }
    }

    private void OnDisable()
    {
        if (spriteRenderers == null)
            return;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = originalColors[i];
        }
    }
}
