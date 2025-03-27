using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossShip : MonoBehaviour
{
    [SerializeField] private float bossHealth;
    [SerializeField] private float bossDamage;
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private GameObject bossDiedText;

    [Header("Enemy Projectile: ")]
    [SerializeField] private float enemyProjectileSpeed;
    [SerializeField] private float enemyProjectileRepeatTime;
    [SerializeField] private float enemyProjectileLifeTime;

    [SerializeField] private GameObject enemyProjectilePreFab;
    [SerializeField] private GameObject enemyHeavyProjectilePreFab;

    [SerializeField] private Transform enemyProjectileStartPossition_1;
    [SerializeField] private Transform enemyProjectileStartPossition_2;
    [SerializeField] private Transform enemyProjectileStartPossition_3;

    private int enemyGunChange = 1;
    private GameObject playerPossiton;
    private Rigidbody2D enemyProjectileRb;
    private int bossAttackSquence = 0;
    private bool bossDeath = false;
    private float totalbossHealth;

    private void Start()
    {
        totalbossHealth = bossHealth;
        //InvokeRepeating(nameof(ShootBullets), enemyProjectileStartTime, enemyProjectileRepeatTime);
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (bossDeath)
            {
                SceneManager.LoadScene(3);
            }
        }

        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            BossTakeDamage(SaveData.instance.playerShipDamage);
            Destroy(collision.gameObject);
        }


    }

    private bool toggle = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (toggle)
            {
                enemyGunChange = 0;
                toggle = false;
            }
            else
            {
                enemyGunChange = 1;
                toggle = true;
            }

        }
    }

    public void StartBossSquence()
    {
        StartCoroutine(BossAttackSquence(enemyProjectileRepeatTime));
    }


    private void ShootBullets()
    {
        if (enemyGunChange == 1)
        {
            GameObject newEnemyProjectile_1 = Instantiate(enemyProjectilePreFab, enemyProjectileStartPossition_1.position, Quaternion.identity);
            GameObject newEnemyProjectile_2 = Instantiate(enemyProjectilePreFab, enemyProjectileStartPossition_2.position, Quaternion.identity);
            playerPossiton = GameObject.FindGameObjectWithTag("Player");

            Vector3 direction_1 = playerPossiton.transform.position - enemyProjectileStartPossition_1.transform.position;
            newEnemyProjectile_1.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction_1.x, direction_1.y).normalized * enemyProjectileSpeed;
            Vector3 direction_2 = playerPossiton.transform.position - enemyProjectileStartPossition_2.transform.position;
            newEnemyProjectile_2.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction_2.x, direction_2.y).normalized * enemyProjectileSpeed;


            float rot_1 = Mathf.Atan2(-direction_1.y, -direction_1.x) * Mathf.Rad2Deg;
            newEnemyProjectile_1.transform.rotation = Quaternion.Euler(0, 0, rot_1 + 90);
            float rot_2 = Mathf.Atan2(-direction_2.y, -direction_2.x) * Mathf.Rad2Deg;
            newEnemyProjectile_2.transform.rotation = Quaternion.Euler(0, 0, rot_2 + 90);

            Destroy(newEnemyProjectile_1, enemyProjectileLifeTime);
            Destroy(newEnemyProjectile_2, enemyProjectileLifeTime);

        }
        else
        {
            ShootTripleProjectiles(enemyProjectileStartPossition_1);
            ShootTripleProjectiles(enemyProjectileStartPossition_2);
            ShootTripleProjectiles(enemyProjectileStartPossition_3);

        }


    }

    private void ShootTripleProjectiles(Transform position)
    {
        GameObject newProjectile1 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.identity);
        GameObject newProjectile2 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.identity);
        GameObject newProjectile3 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.identity);

        Vector3 transformDown = transform.up * -1.0f;

        float angleOffset = 30f; // Angle in degrees
        Vector2 shootDirection = Quaternion.Euler(0, 0, angleOffset) * transformDown;
        Vector2 shootDirection2 = Quaternion.Euler(0, 0, -angleOffset) * transformDown;

        newProjectile1.GetComponent<Rigidbody2D>().AddForce(transformDown * enemyProjectileSpeed, ForceMode2D.Impulse);
        newProjectile2.GetComponent<Rigidbody2D>().AddForce(shootDirection * enemyProjectileSpeed, ForceMode2D.Impulse);
        newProjectile3.GetComponent<Rigidbody2D>().AddForce(shootDirection2 * enemyProjectileSpeed, ForceMode2D.Impulse);

        Destroy(newProjectile1, enemyProjectileLifeTime);
        Destroy(newProjectile2, enemyProjectileLifeTime);
        Destroy(newProjectile3, enemyProjectileLifeTime);
    }

    private void UpdateBossHealthBar(float dmg)
    {
        bossHealthBar.fillAmount -= dmg / totalbossHealth;
    }

    private void BossTakeDamage(float dmg)
    {
        if (bossHealth < 0 || bossHealth == 0)
        {
            BossDeath();
        }
        else
        {
            bossHealth -= dmg;
            UpdateBossHealthBar(dmg);
            //UnityEngine.Debug.Log("Boss Health - Total boss Health % = " );
            float bossHealthPercentage = ((bossHealth / totalbossHealth) * 100);


            if (bossHealthPercentage < 100 && bossHealthPercentage > 75)
            {
                bossAttackSquence = 0;
                UnityEngine.Debug.Log("Boss Health% = " + bossHealthPercentage + " Boss Squence Num: " + bossAttackSquence);
                // UnityEngine.Debug.Log("Sequence is 0 ");

            }
            else if (bossHealthPercentage < 75 && bossHealthPercentage > 50)
            {
                bossAttackSquence = 1;
                UnityEngine.Debug.Log("Boss Health% = " + bossHealthPercentage + " Boss Squence Num: " + bossAttackSquence);
                // UnityEngine.Debug.Log("Sequence is 1 ");

            }
            else if (bossHealthPercentage < 50 && bossHealthPercentage > 25)
            {
                bossAttackSquence = 2;
                UnityEngine.Debug.Log("Boss Health% = " + bossHealthPercentage + " Boss Squence Num: " + bossAttackSquence);
                // UnityEngine.Debug.Log("Sequence is 2 ");


            }
            else if (bossHealthPercentage < 25)
            {
                bossAttackSquence = 3;
                UnityEngine.Debug.Log("Boss Health% = " + bossHealthPercentage + " Boss Squence Num: " + bossAttackSquence);
                //  UnityEngine.Debug.Log("Sequence is 3 ");


            }

            //UnityEngine.Debug.Log("Sequence is 0 ");

        }
    }

    private void BossDeath()
    {
        bossDeath = true;
        bossDiedText.SetActive(true);
    }


    IEnumerator BossAttackSquence(float wait)
    {
        bool toggle = true;
        // It runs every frame like update but with given delay
        while (true)
        {
            switch (bossAttackSquence)
            {
                case 0:
                    enemyGunChange = 1;
                    break;
                case 1:
                    enemyGunChange = 0;
                    break;
                case 2:
                    enemyGunChange = 1;
                    if (toggle)
                    {
                        wait /= 3;
                        enemyProjectileSpeed *= 2;
                        toggle = false;
                    }
                    break;
                case 3:
                    enemyGunChange = 0;
                    break;

            }

            ShootBullets();
            yield return new WaitForSeconds(wait);

            if (bossDeath)
            {
                break;
            }

        }

    }


}

/*
 * 

// Start is called before the first frame update
Unity Message | 0 references
void Start()

enemyProjectileRb = GetComponent<Rigidbody2D>();
player = GameObject. FindGameObjectWithTag("Player");

Vector3 direction_1 = player. transform. position - transform. position;
enemyProjectileRb.velocity = new Vector2(direction_1.x, direction_1.y).normalized * force;

float rot_1 = Mathf.Atan2(-direction_1.y, -direction_1.x) * Mathf.Rad2Deg;
transform.rotation = Quaternion. Euler(0, 0, rot_1);
 

/---------------------------------------------------------------------
 private GameObject player;
private Rigidbody2D enemyProjectileRb;

 */