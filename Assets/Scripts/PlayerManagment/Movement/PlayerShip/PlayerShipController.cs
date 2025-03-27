using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerShipController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private Vector2 movementDirection;
    private PlayerInput playerInputAction;

    [Header("Player Stats: ")]
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private Transform[] playerProjectileSpawn;
    [SerializeField] private float playerProjectileSpeed;
    [SerializeField] private float playerProjectileLifetime;
    [SerializeField] private GameObject playerProjectile;
    [SerializeField] private float playerRespawnTime;
    [SerializeField] private float playerFireRate;
    [Space(10)]

    [SerializeField] private GameObject pauseMenuPanel;
    private bool togglePauseMenu = true;
    private bool toggleAttack = true;
    private bool playerInvulnerability = false;
    private bool multipleProjectile = false;
    private bool toggleProjectilePosition;
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerDamage;
    private float totalPlayerHealth;
    [Space(10)]
    [Header("Audio: ")]
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private AudioSource destroyedSFX;
    [SerializeField] private AudioSource gameMusic;
    [Space(10)]

    [Header("Debug Player: ")]
    [SerializeField] private bool debugMultiProjectiles;


    private InputAction move;
    private InputAction fire;
    private InputAction escape;


    public static event Action OnPlayerRespawn;
    public static event Action OnPlayerTakeDamage;
    public static event Action OnPlayerDeath;



    private float damageTaken;
    private Transform currentProjectilePosition;
    public float GetPlayerHealth()
    {
        return totalPlayerHealth;
    }

    public float GetDamageTaken()
    {
        return damageTaken;
    }


    void Awake()
    {
        totalPlayerHealth = playerHealth;

        if (SaveData.instance.newGame)
        {
            SaveData.instance.newGame = false;
            SaveData.instance.playerShipDamage = playerDamage;
            SaveData.instance.playerShipHealth = playerHealth;
            SaveData.instance.playerShipTotalHealth = totalPlayerHealth;
            SaveData.instance.playerShipMoveSpeed = playerMoveSpeed;
            SaveData.instance.playerShipProjectileSpeed = playerProjectileSpeed;
            SaveData.instance.playerShipMultishot = false;
            SaveData.instance.playerShipFireRate = playerFireRate;

            //playerShipMultishot
        }
        else
        {
            Debug.Log("Players Heath is updated, TotalHealth: " + SaveData.instance.playerShipTotalHealth + " CurrentHealth: " + SaveData.instance.playerShipHealth);
            playerDamage = SaveData.instance.playerShipDamage;
            playerHealth = SaveData.instance.playerShipHealth;
            totalPlayerHealth = SaveData.instance.playerShipTotalHealth;
            multipleProjectile = SaveData.instance.playerShipMultishot;
            playerFireRate = SaveData.instance.playerShipFireRate;
        }


        playerRigidbody = GetComponent<Rigidbody2D>();
        playerInputAction = new PlayerInput();
        move = playerInputAction.PlayerShip.Move;
        fire = playerInputAction.PlayerShip.Attack;
        escape = playerInputAction.PlayerShip.Escape;
        Physics.IgnoreLayerCollision(7, 6);
        //Physics.IgnoreLayerCollision(6, 6);
        //--------------------------------------------------
        if (debugMultiProjectiles)
        {
            multipleProjectile = true;
        }

    }


    private void OnEnable()
    {
        move.Enable();
        fire.Enable();
        fire.performed += Attack;
        escape.Enable();
        escape.performed += PauseGame;

        OnPlayerTakeDamage += TakeDamage;
        OnPlayerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        escape.Disable();

        OnPlayerTakeDamage -= TakeDamage;
        OnPlayerDeath -= PlayerDeath;


    }

    private void Start()
    {
        currentProjectilePosition = playerProjectileSpawn[0];
    }

    private void Move()
    {
        movementDirection = move.ReadValue<Vector2>();
        playerRigidbody.linearVelocity = new Vector2(movementDirection.x * playerMoveSpeed, movementDirection.y * playerMoveSpeed);

    }


    void Update()
    {
        Move();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (toggleAttack)
        {
            shootSFX.Play();
            //Smaller the playerFireRate faster player shoots //ideal 0.7
            StartCoroutine(PlayerAttackDelay(playerFireRate));
        }

    }

    private void ShootProjectile()
    {
        if (toggleProjectilePosition)
        {
            currentProjectilePosition = playerProjectileSpawn[0];
            toggleProjectilePosition = false;
        }
        else
        {
            currentProjectilePosition = playerProjectileSpawn[1];
            toggleProjectilePosition = true;

        }


        if (multipleProjectile)
        {
            GameObject newProjectile1 = Instantiate(playerProjectile, playerProjectileSpawn[2].position, Quaternion.identity);
            GameObject newProjectile2 = Instantiate(playerProjectile, playerProjectileSpawn[0].position, Quaternion.Euler(0, 0, 30));
            GameObject newProjectile3 = Instantiate(playerProjectile, playerProjectileSpawn[1].position, Quaternion.Euler(0, 0, -30));


            float angleOffset = 30f; // Angle in degrees
            Vector2 shootDirection = Quaternion.Euler(0, 0, angleOffset) * transform.up;
            Vector2 shootDirection2 = Quaternion.Euler(0, 0, -angleOffset) * transform.up;

            newProjectile1.GetComponent<Rigidbody2D>().AddForce(transform.up * playerProjectileSpeed, ForceMode2D.Impulse);
            newProjectile1.GetComponent<PlayerProjectile>().SetPlayerDamage(playerDamage);
            newProjectile2.GetComponent<Rigidbody2D>().AddForce(shootDirection * playerProjectileSpeed, ForceMode2D.Impulse);
            newProjectile2.GetComponent<PlayerProjectile>().SetPlayerDamage(playerDamage);
            newProjectile3.GetComponent<Rigidbody2D>().AddForce(shootDirection2 * playerProjectileSpeed, ForceMode2D.Impulse);
            newProjectile3.GetComponent<PlayerProjectile>().SetPlayerDamage(playerDamage);

            Destroy(newProjectile1, playerProjectileLifetime);
            Destroy(newProjectile2, playerProjectileLifetime);
            Destroy(newProjectile3, playerProjectileLifetime);
        }
        else
        {
            //Debug.Log("fire");
            GameObject newProjectile = Instantiate(playerProjectile, currentProjectilePosition.position, Quaternion.identity);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(transform.up * playerProjectileSpeed, ForceMode2D.Impulse);
            newProjectile.GetComponent<PlayerProjectile>().SetPlayerDamage(playerDamage);

            Destroy(newProjectile, playerProjectileLifetime);

        }
    }



    private void TakeDamage()
    {

        SaveData.instance.playerShipHealth -= damageTaken;
        StartCoroutine(PlayerTakeDamageEffect());
        Debug.Log("Player:\n Damage taken:" + damageTaken + " Current Health: " + SaveData.instance.playerShipHealth);

        if (SaveData.instance.playerShipHealth == 0 || SaveData.instance.playerShipHealth < 0)
        {
            OnPlayerDeath?.Invoke();
        }

    }


    IEnumerator PlayerTakeDamageEffect()
    {
        GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask("Enemy");
        //Physics.IgnoreLayerCollision(7, 8, true);
        //GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        //GetComponent<BoxCollider2D>().enabled = true;
        //Physics.IgnoreLayerCollision(7, 8, false);
        GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask("Nothing");

    }




    private void PlayerDeath()
    {
        destroyedSFX.Play();
        gameMusic.Stop();
        //Destroy(gameObject);
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        move.Disable();
        fire.Disable();
        escape.Disable();
        transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        OnPlayerRespawn?.Invoke();
    }



    public void PlayerRespawn()
    {
        //StartCoroutine(PlayerRespawnDelay(wait));

        SaveData.instance.playerShipHealth = SaveData.instance.playerShipTotalHealth / 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    IEnumerator PlayerRespawnDelay(float wait)
    {
        move.Disable();
        fire.Disable();
        escape.Disable();
        transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        SaveData.instance.playerShipHealth = SaveData.instance.playerShipTotalHealth / 2;
        while (wait > 0)
        {
            //Debug.Log("Time remaining: " + wait);
            yield return new WaitForSeconds(1f);
            wait--;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator PlayerAttackDelay(float wait)
    {
        toggleAttack = false;
        ShootProjectile();
        yield return new WaitForSeconds(wait);
        toggleAttack = true;
    }





    private void PauseGame(InputAction.CallbackContext context)
    {
        if (togglePauseMenu)
        {
            pauseMenuPanel.SetActive(false);
            togglePauseMenu = false;
            Time.timeScale = 1f;

        }
        else
        {
            pauseMenuPanel.SetActive(true);
            togglePauseMenu = true;
            Time.timeScale = 0f;

        }

    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damageTaken = collision.gameObject.GetComponent<EnemyShip>().GetEnemyDamage();
            Destroy(collision.gameObject);
            OnPlayerTakeDamage?.Invoke();

        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            //OnPlayerDeath?.Invoke();

        }

        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            damageTaken = collision.gameObject.GetComponent<EnemyProjectile>().EnemyProjectileDamage;
            //OnPlayerDeath?.Invoke();
            Destroy(collision.gameObject);
            OnPlayerTakeDamage?.Invoke();

        }




    }




}


/*
     private void FixedUpdate()
    {
        //playerRigidbody.linearVelocity = movementDirection * playerMoveSpeed * Time.deltaTime ;


    }


    private bool togglePauseMenu = true;

    public void RepairShip(int health)
    {

    }


    public void UpgradeShip(int dmg, int health, float speed)
    {
        playerHealth = playerHealth + health;
        playerDamage = playerDamage + dmg;
        playerMoveSpeed = playerMoveSpeed + speed;
    }

    public void UpgradeProjectile(float projectileSpeed, bool multipleProjectiles)
    {
        multipleProjectile = multipleProjectiles;
        playerProjectileSpeed = projectileSpeed;
    }

 
 */