using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Game/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Общее")]
    public int maxHP = 3;
    public int damage = 1;

    [Header("Зрение")]
    public float visionRange = 6f;
    public float viewAngle = 60f;

    [Header("Движение (Patroller)")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float loseSightTime = 2f;

    [Header("Атака")]
    public float attackRange = 1.2f;
    public float windUpTime = 0.5f;
    public float cooldown = 1.5f;

    [Header("Рывок (Charger)")]
    public float chargeSpeed = 9f;
    public float chargeTime = 0.6f;
    public float recoverTime = 1.5f;
    public float hitRadius = 0.9f;
}