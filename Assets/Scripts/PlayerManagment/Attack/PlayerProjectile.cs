using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float projectileDamage;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
        

        }
    }



    public void SetPlayerDamage(float damage)
    {
        projectileDamage = damage;
    }

    public float GetPlayerDamage()
    {
        return projectileDamage;
    }




}
