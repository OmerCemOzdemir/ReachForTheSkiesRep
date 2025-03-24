using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float enemyProjectileDamage;
    public float EnemyProjectileDamage { get => enemyProjectileDamage; set => enemyProjectileDamage = value; }
}
