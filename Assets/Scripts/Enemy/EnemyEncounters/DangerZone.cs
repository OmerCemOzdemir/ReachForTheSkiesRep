using System;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    // [SerializeField] private int enemyEncounterRate;
    [SerializeField] private float enemyHealth;
    [SerializeField] private float enemyDamage;

    public float EnemyHealth { get => enemyHealth; set => enemyHealth = value; }
    public float EnemyDamage { get => enemyDamage; set => enemyDamage = value; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // RandomEncounter();
            //Destroy(gameObject, 2);
        }
    }

    private void RandomEncounter()
    {
        int rand = UnityEngine.Random.Range(1, 10);
        Debug.Log("Rand: " + rand);

        if (rand == 5)
        {
            // onRandomEncounter?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

}
