using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public sealed class PlayerStats : ScriptableObject
{
    public float moveSpeed;
    public float jumpForce;
    public float fallLimit;
    public int maxHealth;
    public float invincibilityDuration;
    public float blinkInterval;
    public float respawnDelay;
    public int damage;
    public float attackRange;
}
