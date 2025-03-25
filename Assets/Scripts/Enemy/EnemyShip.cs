using System;
using System.Collections;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseDamage;

    [SerializeField] private GameObject resourceMaterial;
    [SerializeField] private int materialScarce;
    [SerializeField] private int materialValue;
    [SerializeField] private EnemyShipType enemyShipType;
    [SerializeField] private GameObject enemyProjectile;
    [HideInInspector] public AudioSource enemyShipDestroySFX;

    //public static event Action onEnemyTakeDamage;
    //public static event Action onEnemyDeath;

    private float damageTaken;
    private bool enemyAttack = true;
    public float GetEnemyDamage()
    {
        return baseDamage;
    }

    private void Start()
    {
        switch (enemyShipType)
        {
            case EnemyShipType.Basic:
                break;
            case EnemyShipType.Intermediate:
                //StartCoroutine(EnemyAttackSquence(2));
                break;
            case EnemyShipType.Advance:
                StartCoroutine(EnemyAttackSquence(5f));
                break;
        }
    }

    IEnumerator EnemyAttackSquence(float fireRate)
    {
        while (true)
        {
            if (enemyAttack)
            {
                EnemyAttack();
            }
            yield return new WaitForSeconds(fireRate);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            //onEnemyTakeDamage?.Invoke();
            Destroy(collision.gameObject);
            damageTaken = collision.gameObject.GetComponent<PlayerProjectile>().GetPlayerDamage();
            EnemyTakeDamage();

            //Debug.Log("it enters");

        }

    }

    private void EnemyAttack()
    {
        //Debug.Log("fire");
        GameObject newProjectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody2D>().AddForce(transform.up * 1.5f, ForceMode2D.Impulse);
        newProjectile.GetComponent<EnemyProjectile>().EnemyProjectileDamage = 10;

        Destroy(newProjectile, 7);
    }

    private void EnemyTakeDamage()
    {
        baseHealth -= damageTaken;
        StartCoroutine(EnemyTakeDamageEffect());
        if (baseHealth == 0 || baseHealth < 0)
        {
            StartCoroutine(EnemyTakeDamageEffect());
            //EnemyDeath();
            //StopCoroutine(EnemyAttackSquence(2f));
            enemyAttack = false;
            StartCoroutine(DelayOnEnemyDeath(GetComponentInChildren<ParticleSystem>().main.duration));
        }
        //Debug.Log("Enemy:\n damage taken:" + damageTaken + " Current Health: " + baseHealth);

    }

    IEnumerator EnemyTakeDamageEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator DelayOnEnemyDeath(float wait)
    {
        if (enemyShipDestroySFX != null)
        {
            enemyShipDestroySFX.Play();
        }
        //enemyParticleSystem.Play();
        SpawnResourceMat();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(wait + 1f);
        EnemyDeath();
    }

    private void SpawnResourceMat()
    {
        int rand = UnityEngine.Random.Range(1, materialScarce);
        int randMat = UnityEngine.Random.Range(1, 20);
        int randMatVal = UnityEngine.Random.Range(1, materialValue);
        int chosenMat = 2;

        if (randMat == 2 || randMat == 3 || randMat == 4 || randMat == 5)
        {
            chosenMat = 1;

        }
        else if (randMat == 6 || randMat == 7 || randMat == 8 || randMat == 9 || randMat == 10 || randMat == 11 || randMat == 12
            || randMat == 13 || randMat == 14)
        {
            chosenMat = 2;

        }
        else if (randMat == 15 || randMat == 16 || randMat == 17 || randMat == 18 || randMat == 19)
        {
            chosenMat = 3;

        }

        if (rand == 2)
        {
            Debug.Log("Random Mat : " + randMat);
            GameObject newResource;
            switch (chosenMat)
            {
                case 1:
                    newResource = Instantiate(resourceMaterial, transform.position, Quaternion.identity);
                    newResource.GetComponent<ResourceMaterial>().ResourceType = Resource.Organic;
                    newResource.GetComponent<ResourceMaterial>().ResourceValue = randMatVal;
                    Destroy(newResource, 10);

                    break;
                case 2:
                    newResource = Instantiate(resourceMaterial, transform.position, Quaternion.identity);
                    newResource.GetComponent<ResourceMaterial>().ResourceType = Resource.MetalScrap;
                    newResource.GetComponent<ResourceMaterial>().ResourceValue = randMatVal;
                    Destroy(newResource, 10);

                    break;
                case 3:
                    newResource = Instantiate(resourceMaterial, transform.position, Quaternion.identity);
                    newResource.GetComponent<ResourceMaterial>().ResourceType = Resource.Chemical;
                    newResource.GetComponent<ResourceMaterial>().ResourceValue = randMatVal;
                    Destroy(newResource, 10);

                    break;
            }

        }

    }

    private void EnemyDeath()
    {
        Destroy(gameObject);
    }


}

enum EnemyShipType
{
    Basic,
    Intermediate,
    Advance
}