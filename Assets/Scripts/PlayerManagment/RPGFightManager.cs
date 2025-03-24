using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RPGFightManager : MonoBehaviour
{
    [SerializeField] private GameObject specialPanel;
    [SerializeField] private GameObject itemsPanel;
    [SerializeField] private GameObject RPGBattlePanel;
    [SerializeField] private GameObject playerBlockPanel;
    [SerializeField] private GameObject playerGuardIcon;
    [SerializeField] private GameObject RPGGameOverPanel;


    [SerializeField] private TextMeshProUGUI RPGGameOverCountdown;
    [SerializeField] private TextMeshProUGUI enemyType;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI playerEnergyText;
    [SerializeField] private TextMeshProUGUI healthBoosterText;
    [SerializeField] private TextMeshProUGUI energyBoosterText;
    [SerializeField] private TextMeshProUGUI damageBoosterText;

    [SerializeField] private AudioSource battleMusic;
    [SerializeField] private AudioSource normalMusic;
    [SerializeField] private AudioSource enemyEncounterSFX;
    [SerializeField] private AudioSource enemyAttackSFX;

    [SerializeField] private RPGPlayer RPGPlayer;
    [SerializeField] private Animator battleAnimator;

    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerEnergyBar;


    [SerializeField] private float turnTime;
    [SerializeField] private float playerHealingAbility;
    public static event Action onBattleProgress;
    public static event Action onPlayerWin;
    public static event Action onPlayerLose;

    //[Header("RPG Enemy Info: ")]
    private float enemyHealth;
    private float enemyBaseAttack;
    private bool isBoss = false;
    private int enemyAttackRandomizer;


    private bool toggleSpecial = true;
    private bool toggleItems = true;
    private bool playerOnGuard = false;
    private bool enemyDebuff = false;
    private int enemyDebuffCountdown = 3;

    private float totalPlayerHealth;
    private float totalPlayerEnergy;
    private float totalEnemyHealth;

    public float GetTotalPlayerHealth()
    {
        return totalPlayerHealth;
    }
    public float GetTotalPlayerEnergy()
    {
        return totalPlayerEnergy;
    }



    private void Start()
    {
        totalPlayerHealth = RPGPlayer.PlayerHealth;
        totalPlayerEnergy = RPGPlayer.PlayerEnergy;
        totalEnemyHealth = enemyHealth;
        UpdateInfoText();
    }

    private void PlayerActivatedAbility(int ability)
    {
        switch (ability)
        {
            case 1:
                StartCoroutine(PlayerDelayAction(turnTime, ability));

                break;
            case 2:
                StartCoroutine(PlayerDelayAction(turnTime, ability));

                break;
            case 3:
                StartCoroutine(PlayerDelayAction(turnTime, ability));
                Debug.Log("Will be Added");
                break;
        }
        UpdateInfoText();

    }

    private void PlayerUsedAbility(int ability)
    {
        switch (ability)
        {
            case 1:


                break;
            case 2:

                break;
            case 3:
                Debug.Log("Will be Added");
                break;
        }
        // UpdateInfoText();

    }

    private void PlayerTakeDamage(float realDmg)
    {
        float dmg = realDmg / 2;
        if (SaveData.instance.playerRPGHealth <= 0)
        {
            //UnityEngine.Debug.Log("WTF happend");
            onPlayerLose?.Invoke();
        }
        else
        {
            if (playerOnGuard)
            {
                SaveData.instance.playerRPGHealth -= dmg;
                playerOnGuard = false;
                UpdatePlayerGuardIcon(0);
            }
            else
            {
                SaveData.instance.playerRPGHealth -= realDmg;
            }
        }
        Debug.Log("Player Took damage");
        UpdateAllInformation();

    }

    private void EnemyTakeDamage(float dmg)
    {
        if (enemyDebuff)
        {
            enemyHealth -= dmg * 3;
            Debug.Log("Enemy Took This much dmg: " + dmg * 3);
            UpdateAllInformation();
            StartCoroutine(EnemyDelayAction(turnTime));
        }
        else
        {
            enemyHealth -= dmg;
            UpdateAllInformation();
            StartCoroutine(EnemyDelayAction(turnTime));
        }
    }

    private void EnemyAttack(int randomize)
    {
        switch (randomize)
        {
            case 0:
                PlayerTakeDamage(enemyBaseAttack);
                break;
            case 1:
                PlayerTakeDamage(enemyBaseAttack * 2);

                break;
        }

    }

    IEnumerator PlayerDelayAction(float wait, int ability)
    {
        UpdateInfoText();

        //  UnityEngine.Debug.Log("Player used Ability ");
        playerBlockPanel.SetActive(true);
        CloseAllPanels();
        enemyAttackRandomizer = UnityEngine.Random.Range(0, 2);
        yield return new WaitForSeconds(wait);
        if (ability == 2)
        {
            if (SaveData.instance.playerRPGEnergy > 50)
            {
                //EnemyTakeDamage(SaveData.instance.playerRPGDamage * 2);
                SaveData.instance.playerRPGEnergy -= 50;
                EnemyTakeDamage(SaveData.instance.playerRPGDamage * 2);
            }
            else
            {
                Debug.Log("Not Enough Energy to Cast");

            }
        }
        else if (ability == 1)
        {
            if (SaveData.instance.playerRPGEnergy > 30)
            {
                if (SaveData.instance.playerRPGHealth < totalPlayerHealth)
                {
                    float tempHealth = SaveData.instance.playerRPGHealth;
                    float tempResultHealth = tempHealth + playerHealingAbility;
                    if (tempResultHealth > totalPlayerHealth)
                    {
                        SaveData.instance.playerRPGHealth = totalPlayerHealth;
                        Debug.Log("Player Health Info 1: " + SaveData.instance.playerRPGHealth + "/" + totalPlayerHealth);
                    }
                    else
                    {
                        SaveData.instance.playerRPGHealth = tempResultHealth;
                        Debug.Log("Player Health Info 2: " + SaveData.instance.playerRPGHealth + "/" + tempResultHealth);

                    }
                    UpdateAllInformation();

                    SaveData.instance.playerRPGEnergy -= 30;
                    // Debug.Log("Player Health: " + SaveData.instance.playerRPGHealth );
                }
                else { Debug.Log("Player Health is Already full"); }
            }
            else
            {
                Debug.Log("Not Enough Energy to Cast");

            }
            Transtion();
        }
        else if (ability == 3)
        {
            if (SaveData.instance.playerRPGEnergy > 70)
            {
                enemyDebuff = true;
                enemyDebuffCountdown = 3;
                Debug.Log("Debuff Casted: " + enemyDebuff);

                SaveData.instance.playerRPGEnergy -= 70;
            }
            else
            {
                Debug.Log("Not Enough Energy to Cast");

            }
            Transtion();
        }
        playerBlockPanel.SetActive(false);
        //PlayerUsedAbility(ability);
        // UnityEngine.Debug.Log("Player ability Ended ");
    }

    private void Transtion()
    {

        StartCoroutine(EnemyDelayAction(turnTime));

    }

    IEnumerator EnemyDelayAction(float wait)
    {
        //UpdateInfoText();
        // UnityEngine.Debug.Log("Enemy Attacks Player ");
        enemyAttackSFX.Play();
        playerBlockPanel.SetActive(true);
        enemyAttackRandomizer = UnityEngine.Random.Range(0, 2);
        CloseAllPanels();
        yield return new WaitForSeconds(wait);
        EnemyAttack(enemyAttackRandomizer);
        playerBlockPanel.SetActive(false);
        // UnityEngine.Debug.Log("Enemy Health: " + enemyHealth + " Player Health: " + RPGPlayer.PlayerHealth + " EnemyRandomizer: " + enemyAttackRandomizer);
        // UnityEngine.Debug.Log("Enemy Attack Ended ");
        if (enemyDebuffCountdown == 0)
        {
            enemyDebuff = false;
        }
        else
        {
            if (enemyDebuff)
            {
                enemyDebuffCountdown--;
            }
        }

        if (enemyHealth <= 0)
        {
            onPlayerWin?.Invoke();
        }
        else
        {
            onBattleProgress?.Invoke();
        }


    }

    private void UpdatePlayerGuardIcon(float nonDMG)
    {

        if (playerOnGuard)
        {
            playerGuardIcon.SetActive(true);
        }
        else
        {
            playerGuardIcon.SetActive(false);

        }
    }

    private void UpdateAllBars()
    {
        UpdateEnemyHealthBar();
        UpdatePlayerHealthBar();
        UpdatePlayerEnergyBar();
    }

    private void UpdateEnemyHealthBar()
    {
        enemyHealthBar.fillAmount = (float)(enemyHealth / totalEnemyHealth);
        //  UnityEngine.Debug.Log("Enemy Health %: " + dmg / enemyHealth);

    }

    private void UpdatePlayerHealthBar()
    {
        float fillPercentage = SaveData.instance.playerRPGHealth / totalPlayerHealth;
        playerHealthBar.fillAmount = fillPercentage;
        //UnityEngine.Debug.Log("Player Health %: " + (totalPlayerHealth / SaveData.instance.playerRPGHealth) + "\nPlayer health: " + SaveData.instance.playerRPGHealth + "\n Total Player health: " + totalPlayerHealth);

    }

    private void UpdatePlayerEnergyBar()
    {
        playerEnergyBar.fillAmount = SaveData.instance.playerRPGEnergy / totalPlayerEnergy;
        //  UnityEngine.Debug.Log("Player Health %: " + totalPlayerHealth / SaveData.instance.playerRPGHealth + "Player health: " + SaveData.instance.playerRPGHealth);

    }

    private void UpdateInfoText()
    {
        enemyHealthText.text = "" + enemyHealth + " / " + totalEnemyHealth;
        playerHealthText.text = "" + SaveData.instance.playerRPGHealth + " / " + totalPlayerHealth;
        playerEnergyText.text = "" + SaveData.instance.playerRPGEnergy + " / " + totalPlayerEnergy;

    }

    private void UpdateAllInformation()
    {
        UpdateAllBars();
        UpdateInfoText();
        healthBoosterText.text = "" + SaveData.instance.playerRPGHealthBooster;
        energyBoosterText.text = "" + SaveData.instance.playerRPGEnergyBooster;
        damageBoosterText.text = "" + SaveData.instance.playerRPGDamageBooster;
    }

    public void OpenSpecialPanel()
    {
        if (toggleSpecial)
        {
            specialPanel.SetActive(true);
            itemsPanel.SetActive(false);
            toggleSpecial = false;
        }
        else
        {
            specialPanel.SetActive(false);
            toggleSpecial = true;
        }


    }

    public void OpenItemsPanel()
    {
        if (toggleItems)
        {
            specialPanel.SetActive(false);
            itemsPanel.SetActive(true);
            toggleItems = false;
        }
        else
        {
            itemsPanel.SetActive(false);
            toggleItems = true;
        }


    }

    public void CloseAllPanels()
    {
        specialPanel.SetActive(false);
        itemsPanel.SetActive(false);
        toggleItems = true;
        toggleSpecial = true;
    }

    public void UseHealthBooster()
    {
        if (SaveData.instance.playerRPGHealthBooster > 0)
        {
            float tempHealth = SaveData.instance.playerRPGHealth;
            tempHealth += 50;
            if (tempHealth > totalPlayerEnergy)
            {
                SaveData.instance.playerRPGHealth = totalPlayerEnergy;
            }
            else
            {
                SaveData.instance.playerRPGHealth = tempHealth;
            }
            SaveData.instance.playerRPGHealthBooster--;

        }
        else
        {
            Debug.Log("Dont have any HealthBoosters");
        }

        UpdateAllInformation();
        CloseAllPanels();
    }

    public void UseDamageBooster()
    {
        if (SaveData.instance.playerRPGDamageBooster > 0)
        {
            SaveData.instance.playerRPGDamage += 10;
            SaveData.instance.playerRPGDamageBooster--;
        }
        else
        {
            Debug.Log("Dont have any DamageBoosters");
        }
        UpdateAllInformation();
        CloseAllPanels();
    }

    public void UseEnergyBooster()
    {
        if (SaveData.instance.playerRPGEnergyBooster > 0)
        {
            float tempEnergy = SaveData.instance.playerRPGEnergy;
            tempEnergy += 50;
            if (tempEnergy > totalPlayerEnergy)
            {
                SaveData.instance.playerRPGEnergy = totalPlayerEnergy;
            }
            else
            {
                SaveData.instance.playerRPGEnergy = tempEnergy;
            }
            SaveData.instance.playerRPGEnergyBooster--;

        }
        else
        {
            Debug.Log("Dont have any EnergyBoosters");
        }
        UpdateAllInformation();
        CloseAllPanels();
    }

    public void OutSideCombatHeal()
    {
        if (SaveData.instance.playerRPGEnergy > 30)
        {
            if (SaveData.instance.playerRPGHealth < totalPlayerHealth)
            {
                float tempHealth = SaveData.instance.playerRPGHealth;
                float tempResultHealth = tempHealth + playerHealingAbility;
                if (tempResultHealth > totalPlayerHealth)
                {
                    SaveData.instance.playerRPGHealth = totalPlayerHealth;
                    Debug.Log("Player Health Info 1: " + SaveData.instance.playerRPGHealth + "/" + totalPlayerHealth);
                }
                else
                {
                    SaveData.instance.playerRPGHealth = tempResultHealth;
                    Debug.Log("Player Health Info 2: " + SaveData.instance.playerRPGHealth + "/" + tempResultHealth);

                }
                UpdateAllInformation();

                SaveData.instance.playerRPGEnergy -= 30;
                // Debug.Log("Player Health: " + SaveData.instance.playerRPGHealth );
                onBattleProgress?.Invoke();

            }
            else { Debug.Log("Player Health is Already full"); }

        }

    }

    private void ResetRPGFight()
    {
        RPGPlayer.PlayerHealth = totalPlayerHealth;
        enemyHealth = totalEnemyHealth;
    }

    private void PlayerWins()
    {
        if (isBoss)
        {
            StopRPGBattle();
            ResetRPGFight();
            SceneManager.LoadScene(4);
        }
        else
        {
            StopRPGBattle();
            ResetRPGFight();
        }

    }



    private void PlayerLoses()
    {
        SaveData.instance.playerRPGHealth = totalPlayerHealth;
        StopRPGBattle();
        StartCoroutine(RPGGameOver(5));
    }

    IEnumerator RPGGameOver(float wait)
    {
        RPGGameOverPanel.SetActive(true);
        while (wait > 0)
        {
            RPGGameOverCountdown.text = "" + wait;
            //Debug.Log("Time remaining: " + wait);
            yield return new WaitForSeconds(1f);
            wait--;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartRPGBattle(bool isBoss, float health, float dmg)
    {
        this.isBoss = isBoss;
        enemyHealth = health;
        enemyBaseAttack = dmg;
        totalEnemyHealth = enemyHealth;
        battleAnimator.ResetTrigger("StopAnim");
        battleAnimator.SetTrigger("StartAnim");
        StartCoroutine(PlayBattleMusic());

        if (isBoss)
        {
            enemyType.text = "Boss";
        }
        else
        {
            enemyType.text = "Enemy";
        }
        //RPGBattlePanel.SetActive(true);
        //UpdateInfoText();
        UpdateAllInformation();
    }

    // index 3 on start , index 1 on stop
    IEnumerator PlayBattleMusic()
    {
        enemyEncounterSFX.Play();
        normalMusic.Stop();

        yield return new WaitForSeconds(enemyEncounterSFX.time);
        battleMusic.Play();
    }


    public void StopRPGBattle()
    {
        //RPGBattlePanel.SetActive(false);
        battleAnimator.SetTrigger("StopAnim");
        battleAnimator.ResetTrigger("StartAnim");
        // StartCoroutine(DelayOnAnimationStop());
        normalMusic.Play();
        battleMusic.Stop();
        enemyDebuff = false;
        enemyDebuffCountdown = 3;
        PlayerRPGUIControls.randomEnemyEncounter = false;
        CloseAllPanels();
    }

    private void OnEnable()
    {
        RPGPlayer.onPlayerAttack += EnemyTakeDamage;
        PlayerRPGUIControls.onRandomEncounter += StartRPGBattle;
        onPlayerWin += PlayerWins;
        onPlayerLose += PlayerLoses;
        RPGBoss.onBossEncounter += StartRPGBattle;
        RPGPlayer.onPlayerGuard += UpdatePlayerGuardIcon;
        RPGPlayer.onPlayerGuard += EnemyTakeDamage;
        RPGPlayer.onPlayerAbility += PlayerActivatedAbility;
    }

    private void OnDisable()
    {
        RPGPlayer.onPlayerAttack -= EnemyTakeDamage;
        PlayerRPGUIControls.onRandomEncounter -= StartRPGBattle;
        onPlayerWin -= PlayerWins;
        onPlayerLose -= PlayerLoses;
        RPGBoss.onBossEncounter -= StartRPGBattle;
        RPGPlayer.onPlayerGuard -= UpdatePlayerGuardIcon;
        RPGPlayer.onPlayerGuard -= EnemyTakeDamage;
        RPGPlayer.onPlayerAbility -= PlayerActivatedAbility;

    }

}

public enum RPGEnemyType
{
    Boss,
    Enemy,

}

/*
                 //EnemyTakeDamage(SaveData.instance.playerRPGDamage * 2);
                float tempHealth = SaveData.instance.playerRPGEnergy; 
                float tempResultEnergy = tempHealth -= 50;
                if (tempResultEnergy < 0)
                {
                    No no !!!

                }

 
 */