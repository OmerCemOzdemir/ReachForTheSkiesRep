using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points: ")]
    [SerializeField] private GameObject[] enemySpawnPoints;
    [SerializeField] private GameObject[] enemyShips;
    [Space(10)]

    [Header("First Enemy Info: ")]
    [SerializeField] private int firstEnemyShipHealth;
    [SerializeField] private int firstEnemyShipDamage;
    [Space(10)]


    [Header("Enemy Spawn Info: ")]
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private float enemySpawnTime;
    [SerializeField] private float enemySpawnRateMultiplier;
    [SerializeField] private float enemyShipLifeTime;
    [SerializeField] private float enemyShipSpeed;
    [Space(10)]


    [Header("Boss Stats:  ")]
    [SerializeField] private GameObject enemyBoss;
    [SerializeField] private int bossHealth;
    [SerializeField] private int bossDamage;
    [Space(10)]


    [Header("Misc:  ")]
    [SerializeField] private ShipGameManager shipGameManager;
    [SerializeField] private AudioSource shipDestroyedSFX;

    private int currentStage = 0;

    private void SetNewStage()
    {
        StopAllCoroutines();
        enemySpawnRate = enemySpawnRate / enemySpawnRateMultiplier;
        currentStage++;
        Debug.Log("Stage Clear " + currentStage + " enemySpawnRate: " + enemySpawnRate);
        StartCoroutine(EnemySpawnerRoutine(enemySpawnRate));

    }


    private void SpawnBoss()
    {
        StopAllCoroutines();
        enemyBoss.SetActive(true);
    }


    private Vector2 GetRandomSpawnPoint()
    {
        float newX = UnityEngine.Random.Range(enemySpawnPoints[0].transform.position.x, enemySpawnPoints[1].transform.position.x);
        float newY = UnityEngine.Random.Range(enemySpawnPoints[0].transform.position.y, enemySpawnPoints[1].transform.position.y);

        Vector2 newSpawnPoint = new Vector2(newX, newY);

        return newSpawnPoint;
    }

    private void RandomSpawnEnemy()
    {
        int randomSpawnPoint = UnityEngine.Random.Range(0, 4);
        int randomEnemyShip;

        GameObject newEnemyShip;


        switch (currentStage)
        {
            case 0:
                newEnemyShip = Instantiate(enemyShips[0], GetRandomSpawnPoint(), Quaternion.Euler(0, 0, 180));
                newEnemyShip.GetComponent<Rigidbody2D>().AddForce(-transform.up * enemyShipSpeed, ForceMode2D.Impulse);
                newEnemyShip.GetComponent<EnemyShip>().enemyShipDestroySFX = shipDestroyedSFX;
                Destroy(newEnemyShip, enemyShipLifeTime);

                break;
            case 1:
                randomEnemyShip = UnityEngine.Random.Range(0, 2);
                //  Debug.Log("Stage 1 is playing " + randomEnemyShip);

                newEnemyShip = Instantiate(enemyShips[randomEnemyShip], GetRandomSpawnPoint(), Quaternion.Euler(0, 0, 180));
                newEnemyShip.GetComponent<Rigidbody2D>().AddForce(-transform.up * enemyShipSpeed, ForceMode2D.Impulse);
                newEnemyShip.GetComponent<EnemyShip>().enemyShipDestroySFX = shipDestroyedSFX;

                Destroy(newEnemyShip, enemyShipLifeTime);
                break;
            case 2:
                randomEnemyShip = UnityEngine.Random.Range(0, 3);
                //Debug.Log("Stage 2 is playing " + randomEnemyShip);

                newEnemyShip = Instantiate(enemyShips[randomEnemyShip], GetRandomSpawnPoint(), Quaternion.Euler(0, 0, 180));
                newEnemyShip.GetComponent<Rigidbody2D>().AddForce(-transform.up * enemyShipSpeed, ForceMode2D.Impulse);
                newEnemyShip.GetComponent<EnemyShip>().enemyShipDestroySFX = shipDestroyedSFX;

                Destroy(newEnemyShip, enemyShipLifeTime);
                break;
        }

    }

    IEnumerator EnemySpawnerRoutine(float enemySpawnRate)
    {
        while (true)
        {
            RandomSpawnEnemy();
            // Debug.Log("Cycling... " + Time.time);
            yield return new WaitForSeconds(enemySpawnRate);
        }
    }



    private void Start()
    {
        StartCoroutine(EnemySpawnerRoutine(enemySpawnRate));
        //InvokeRepeating(nameof(RandomSpawnEnemy), enemySpawnRate, enemySpawnTime);
    }

    private void OnEnable()
    {
        ShipGameManager.onStageClear += SetNewStage;
        ShipGameManager.onBossEncounter += SpawnBoss;
    }

    private void OnDisable()
    {
        ShipGameManager.onStageClear -= SetNewStage;
        ShipGameManager.onBossEncounter -= SpawnBoss;


    }


}
//  Debug.Log("Random number" + UnityEngine.Random.Range(0, 3));
