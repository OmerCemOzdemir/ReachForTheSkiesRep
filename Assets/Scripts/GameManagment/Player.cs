using System.Linq.Expressions;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int damage;




    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
           // GameManager.onPlayerTakeDamage?.Invoke();

        }

    }


}
