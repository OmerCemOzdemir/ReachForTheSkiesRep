using System;
using UnityEngine;

public class RPGBoss : MonoBehaviour
{
    [SerializeField] private float bossHealth;
    [SerializeField] private float bossDamage;

    public static event Action<bool, float, float> onBossEncounter;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onBossEncounter?.Invoke(true, bossHealth, bossDamage);
        }
    }




}
