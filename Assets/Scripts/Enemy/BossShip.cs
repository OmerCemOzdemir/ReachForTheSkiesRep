using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossShip : MonoBehaviour
{
    [SerializeField] private float bossHealth;
    [SerializeField] private float bossDamage;
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private GameObject bossDiedText;
    [SerializeField] private float lerpSpeed;

    [Header("Enemy Projectile: ")]
    [SerializeField] private float enemyProjectileSpeed;
    [SerializeField] private float enemyProjectileRepeatTime;
    [SerializeField] private float enemyProjectileLifeTime;

    [SerializeField] private GameObject enemyProjectilePreFab;
    [SerializeField] private GameObject enemyHeavyProjectilePreFab;

    [SerializeField] private Transform enemyProjectileStartPossition_1;
    [SerializeField] private Transform enemyProjectileStartPossition_2;
    [SerializeField] private Transform enemyProjectileStartPossition_3;
    [SerializeField] private Transform enemyProjectileStartPossition_4;

    private int enemyGunChange = 1;
    private GameObject playerPossiton;
    private Rigidbody2D enemyProjectileRb;
    private int bossAttackSquence = 0;
    private bool bossDeath = false;
    private float totalbossHealth;
    private LoaderManager loaderManager;
    private int tripleShootOffSet_1 = 0;
    private int tripleShootOffSet_2 = 0;


    public void LerpObjectToPoint()
    {
        Vector3 endVector = transform.GetChild(0).position;
        Vector3 startVector = transform.position;
        StartCoroutine(LerpObjectCoroutine(startVector, endVector, lerpSpeed));
        //Debug.Log("LerpStarted");
    }


    IEnumerator LerpObjectCoroutine(Vector3 source, Vector3 target, float overTime)
    {
        yield return new WaitForSeconds(10f);
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
        StartCoroutine(BossAttackSquence(enemyProjectileRepeatTime));

    }


    private void Start()
    {
        totalbossHealth = bossHealth;
        //InvokeRepeating(nameof(ShootBullets), enemyProjectileStartTime, enemyProjectileRepeatTime);
        loaderManager = FindAnyObjectByType<LoaderManager>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (bossDeath)
            {
                loaderManager.LoadNextLevel(3);
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
            //ShootTripleProjectiles(enemyProjectileStartPossition_3, true);
           // ShootTripleProjectiles(enemyProjectileStartPossition_4, false);
            ShootTripleProjectiles(enemyProjectileStartPossition_3);
            ShootTripleProjectiles(enemyProjectileStartPossition_4);

        }


    }


    private void ShootTripleProjectiles(Transform position, bool switchGunIndex)
    {
        if (switchGunIndex)
        {
            if (tripleShootOffSet_2 == -50)
            {
                tripleShootOffSet_2 = 0;
            }
            GameObject newProjectile1 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, tripleShootOffSet_2));
            GameObject newProjectile2 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, 30 + tripleShootOffSet_2));
            GameObject newProjectile3 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, -30 - tripleShootOffSet_2));

            // Vector3 newOffSet = new Vector3(tripleShootOffSet_1, 0, 0);

            Vector3 transformDown = transform.up * -1.0f;

            float angleOffset = 30f + tripleShootOffSet_2; // Angle in degrees
            Vector2 shootDirection_1 = Quaternion.Euler(0, 0, tripleShootOffSet_2) * transformDown;
            Vector2 shootDirection_2 = Quaternion.Euler(0, 0, angleOffset ) * transformDown;
            Vector2 shootDirection_3 = Quaternion.Euler(0, 0, -angleOffset ) * transformDown;

            newProjectile1.GetComponent<Rigidbody2D>().AddForce(shootDirection_1 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);
            newProjectile2.GetComponent<Rigidbody2D>().AddForce(shootDirection_2 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);
            newProjectile3.GetComponent<Rigidbody2D>().AddForce(shootDirection_3 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);


            Destroy(newProjectile1, enemyProjectileLifeTime);
            Destroy(newProjectile2, enemyProjectileLifeTime);
            Destroy(newProjectile3, enemyProjectileLifeTime);

            tripleShootOffSet_2 -= 5;


        }
        else
        {
            if (tripleShootOffSet_1 == 50)
            {
                tripleShootOffSet_1 = 0;
            }
            GameObject newProjectile1 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, tripleShootOffSet_1));
            GameObject newProjectile2 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, 30 + tripleShootOffSet_1));
            GameObject newProjectile3 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, -30 + tripleShootOffSet_1));

            // Vector3 newOffSet = new Vector3(tripleShootOffSet_1, 0, 0);

            Vector3 transformDown = transform.up * -1.0f;

            float angleOffset = 30f + tripleShootOffSet_1; // Angle in degrees
            Vector2 shootDirection_1 = Quaternion.Euler(0, 0, tripleShootOffSet_1) * transformDown;
            Vector2 shootDirection_2 = Quaternion.Euler(0, 0, angleOffset ) * transformDown;
            Vector2 shootDirection_3 = Quaternion.Euler(0, 0, -angleOffset ) * transformDown;

            newProjectile1.GetComponent<Rigidbody2D>().AddForce(shootDirection_1 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);
            newProjectile2.GetComponent<Rigidbody2D>().AddForce(shootDirection_2 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);
            newProjectile3.GetComponent<Rigidbody2D>().AddForce(shootDirection_3 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);


            Destroy(newProjectile1, enemyProjectileLifeTime);
            Destroy(newProjectile2, enemyProjectileLifeTime);
            Destroy(newProjectile3, enemyProjectileLifeTime);

            tripleShootOffSet_2 += 5;

        }

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
            StartCoroutine(BossTakeDamageEffect());
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

    IEnumerator BossTakeDamageEffect()
    {
        //Physics.IgnoreLayerCollision(7, 8, true);
        //GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

        //GetComponent<BoxCollider2D>().enabled = true;
        //Physics.IgnoreLayerCollision(7, 8, false);

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

    private void ShootTripleProjectiles(Transform position)
    {

        GameObject newProjectile1 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, 0));
        GameObject newProjectile2 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, 30 + tripleShootOffSet_2));
        GameObject newProjectile3 = Instantiate(enemyHeavyProjectilePreFab, position.position, Quaternion.Euler(0, 0, -30 - tripleShootOffSet_2));

        // Vector3 newOffSet = new Vector3(tripleShootOffSet_1, 0, 0);

        Vector3 transformDown = transform.up * -1.0f;

        float angleOffset = 30f + tripleShootOffSet_2; // Angle in degrees
        Vector2 shootDirection_1 = transformDown;
        Vector2 shootDirection_2 = Quaternion.Euler(0, 0, angleOffset) * transformDown;
        Vector2 shootDirection_3 = Quaternion.Euler(0, 0, -angleOffset) * transformDown;

        newProjectile1.GetComponent<Rigidbody2D>().AddForce(shootDirection_1 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);
        newProjectile2.GetComponent<Rigidbody2D>().AddForce(shootDirection_2 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);
        newProjectile3.GetComponent<Rigidbody2D>().AddForce(shootDirection_3 * enemyProjectileSpeed / 3, ForceMode2D.Impulse);


        Destroy(newProjectile1, enemyProjectileLifeTime);
        Destroy(newProjectile2, enemyProjectileLifeTime);
        Destroy(newProjectile3, enemyProjectileLifeTime);

        tripleShootOffSet_2 += 5;

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