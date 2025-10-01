using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RPGFightManager : MonoBehaviour
{
    [Header("RPG Player Info: ")]
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerAttack;
    [SerializeField] private float playerEnergy;
    [Space(10)]

    [Header("RPG Battle Panels: ")]
    [SerializeField] private GameObject specialPanel;
    [SerializeField] private GameObject itemsPanel;
    [SerializeField] private GameObject RPGBattlePanel;
    [SerializeField] private GameObject playerBlockPanel;
    [SerializeField] private GameObject RPGGameOverPanel;
    [Space(10)]

    [Header("RPG Battle Information: ")]
    [SerializeField] private TextMeshProUGUI enemyType;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI playerEnergyText;
    [SerializeField] private TextMeshProUGUI healthBoosterText;
    [SerializeField] private TextMeshProUGUI energyBoosterText;
    [SerializeField] private TextMeshProUGUI damageBoosterText;
    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerEnergyBar;
    [SerializeField] private GameObject playerGuardIcon;
    [SerializeField] private GameObject enemyChargeIcon;
    [SerializeField] private GameObject enemyDebuffIcon;

    [Space(10)]

    [Header("RPG Battle Item Buttons: ")]
    [SerializeField] private Button healthBooster;
    [SerializeField] private Button damageBooster;
    [SerializeField] private Button energyBooster;
    [Space(10)]


    [Header("RPG Battle Sounds: ")]
    [SerializeField] private AudioSource battleMusic;
    [SerializeField] private AudioSource normalMusic;
    [SerializeField] private AudioSource enemyEncounterSFX;
    [SerializeField] private AudioSource enemyAttackSFX;
    [Space(10)]

    [Header("RPG Battle Other: ")]
    [SerializeField] private Animator battleAnimator;
    [SerializeField] private float turnTime;
    [SerializeField] private float playerHealingAbility;
    [SerializeField] private float turnDelay = 0f;
    [Space(10)]

    [Header("Energy Required Persentage %: ")]
    [SerializeField] private float energyRequiredHeal = 20;
    [SerializeField] private float energyRequiredDouble = 20;
    [SerializeField] private float energyRequiredDebuff = 20;

    public static event Action onBattleProgress;
    public static event Action onPlayerWin;
    public static event Action onPlayerLose;

    //----------------------------------------------------------------------

    private float enemyHealth;
    private float enemyBaseAttack;

    private bool isBoss = false;
    private bool enemyDebuff = false;

    private int enemyAttackRandomizer;
    private int enemyDebuffCountdown = 3;
    private int eneyChargeCountdown = 2;
    private float totalEnemyHealth;

    private bool playerGuard = false;
    private bool enemyCharge = false;
    private bool toggleSpecial = true;
    private bool toggleItems = true;
    private bool continueAction = true;

    private RPGPlayerActions playerAction;
    private RPGEnemyActions enemyActions;
    private RPGPlayerAbilities activeAbility;
    private RPGPlayerItems activeItem;


    private void Start()
    {
        totalEnemyHealth = enemyHealth;
        if (SaveData.instance.RPGnewGame)
        {
            SaveData.instance.playerRPGHealth = playerHealth;
            SaveData.instance.playerRPGDamage = playerAttack;
            SaveData.instance.playerRPGEnergy = playerEnergy;
            SaveData.instance.playerRPGTotalHealth = playerHealth;
            SaveData.instance.playerRPGTotalDamage = playerAttack;
            SaveData.instance.playerRPGTotalEnergy = playerEnergy;
            SaveData.instance.RPGnewGame = false;
        }

        UpdateItemButton();
        UpdateInfoText();
    }

    public void PlayerAbility(int ability)
    {
        activeAbility = (RPGPlayerAbilities)ability;
    }

    public void PlayerItem(int item)
    {
        activeItem = (RPGPlayerItems)item;
    }

    public void PlayerAction(int action)
    {
        playerAction = (RPGPlayerActions)action;
        float hp = SaveData.instance.playerRPGHealth;
        float mp = SaveData.instance.playerRPGEnergy;
        float totalHp = SaveData.instance.playerRPGTotalHealth;
        float totalMp = SaveData.instance.playerRPGTotalEnergy;
        switch (playerAction)
        {
            case RPGPlayerActions.Attack:
                continueAction = true;
                break;
            case RPGPlayerActions.Guard:
                continueAction = true;
                break;
            case RPGPlayerActions.UseAbility:
                switch (activeAbility)
                {
                    case RPGPlayerAbilities.Heal:
                        if (mp <= CalculatePercentage(energyRequiredHeal, totalMp))
                        {
                            InfoPanel.instance.TriggerInfoText("Not Enough Energy", Color.red);
                            continueAction = false;
                        }
                        else
                        {
                            continueAction = true;
                            break;
                        }
                        break;
                    case RPGPlayerAbilities.Double:
                        if (mp <= CalculatePercentage(energyRequiredDouble, totalMp))
                        {
                            continueAction = false;
                            InfoPanel.instance.TriggerInfoText("Not Enough Energy", Color.red);
                        }
                        else
                        {
                            continueAction = true;
                            break;
                        }
                        break;
                    case RPGPlayerAbilities.Debuff:
                        if (mp <= CalculatePercentage(energyRequiredDebuff, totalMp))
                        {
                            continueAction = false;
                            InfoPanel.instance.TriggerInfoText("Not Enough Energy", Color.red);
                        }
                        else
                        {
                            continueAction = true;
                            break;
                        }
                        break;
                }
                break;
            case RPGPlayerActions.UseItem:
                switch (activeItem)
                {
                    case RPGPlayerItems.HealthBooster:
                        if (SaveData.instance.playerRPGHealthBooster <= 0)
                        {
                            continueAction = false;
                            InfoPanel.instance.TriggerInfoText("No Item Left", Color.red);
                        }
                        else
                        {
                            continueAction = true;
                            break;
                        }
                        break;
                    case RPGPlayerItems.EnergyBooster:
                        if (SaveData.instance.playerRPGEnergyBooster <= 0)
                        {
                            continueAction = false;
                            InfoPanel.instance.TriggerInfoText("No Item Left", Color.red);
                        }
                        else
                        {
                            continueAction = true;
                            break;
                        }
                        break;
                    case RPGPlayerItems.DamageBooster:
                        if (SaveData.instance.playerRPGDamageBooster <= 0)
                        {
                            continueAction = false;
                            InfoPanel.instance.TriggerInfoText("No Item Left", Color.red);
                        }
                        else
                        {
                            continueAction = true;
                            break;
                        }
                        break;
                }
                break;
        }

        if (continueAction)
        {
            StartCoroutine(TurnDelayPlayer());
            playerBlockPanel.SetActive(true);
            InfoPanel.instance.TriggerInfoText("Player " + playerAction.ToString(), Color.green);
            //Debug.Log("");
            enemyAttackRandomizer = UnityEngine.Random.Range(1, 5);

            if (itemsPanel.activeSelf)
            {
                Debug.Log("Close the Items panel");
                ToggleItemsPanel();
            }

            if (specialPanel.activeSelf)
            {
                Debug.Log("Close the Ability panel");
                ToggleSpecialPanel();
            }

        }
    }


    private IEnumerator TurnDelayPlayer()
    {
        Debug.Log("Player Turn Delay: " + turnDelay);
        UpdateAllInformation();
        yield return new WaitForSeconds(turnDelay);
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        switch (playerAction)
        {
            case RPGPlayerActions.Attack:
                Debug.Log("Player Attack");
                DamageEnemy(SaveData.instance.playerRPGDamage, 1, enemyDebuff);
                InfoPanel.instance.TriggerInfoText("Player Attacked " + SaveData.instance.playerRPGDamage, Color.green);
                break;
            case RPGPlayerActions.Guard:
                playerGuard = true;
                playerGuardIcon.SetActive(true);
                InfoPanel.instance.TriggerInfoText("Used Guard", Color.green);
                break;
            case RPGPlayerActions.UseAbility:
                UseAbility(activeAbility);
                InfoPanel.instance.TriggerInfoText("Ability Used " + activeAbility.ToString(), Color.green);
                break;
            case RPGPlayerActions.UseItem:
                UseItem(activeItem);
                InfoPanel.instance.TriggerInfoText("Item Used " + activeItem.ToString(), Color.green);
                break;
        }
        UpdateAllInformation();

        StartCoroutine(TurnDelayEnemy());

    }

    private IEnumerator TurnDelayEnemy()
    {
        if (enemyAttackRandomizer >= 2)
        {
            enemyActions = RPGEnemyActions.Attack;
        }
        else
        {
            enemyActions = RPGEnemyActions.Charge;
        }
        //Debug.Log("enemyAttackRandomizer:" + enemyAttackRandomizer + " enemyActions: " + enemyActions);

        if (enemyDebuffCountdown == 0 && enemyDebuff)
        {
            enemyDebuff = false;
            enemyDebuffIcon.SetActive(false);
            enemyDebuffCountdown = 3;
        }
        else
        {
            enemyDebuffCountdown--;
        }

        Debug.Log("Enemy Turn Delay: " + turnDelay);
        yield return new WaitForSeconds(turnDelay);

        if (enemyCharge)
        {
            ChargeEnemy();
            Debug.Log("Enemy is charging");
        }
        else
        {
            EnemyTurn();
        }
        playerBlockPanel.SetActive(false);
        playerGuard = false;
        playerGuardIcon.SetActive(false);
    }

    private void EnemyTurn()
    {
        switch (enemyActions)
        {
            case RPGEnemyActions.Attack:
                DamagePlayer(enemyBaseAttack, 1, playerGuard);
                break;
            case RPGEnemyActions.Charge:
                enemyCharge = true;
                enemyChargeIcon.SetActive(true);
                break;
        }

        UpdateAllInformation();
    }

    //---------------------------------------------------------------------------------------

    private void DamagePlayer(float dmg, float mlp, bool def)
    {
        float hp = SaveData.instance.playerRPGHealth;
        Debug.Log("Player health: " + hp);
        //Debug.Log("Damage Recieved: " + hp);
        InfoPanel.instance.TriggerInfoText("Enemy Attacked " + dmg * mlp, Color.red);

        float newhp;
        if (def)
        {
            newhp = hp - ((dmg * mlp) / 2);

        }
        else
        {
            newhp = hp - (dmg * mlp);

        }

        if (newhp <= 0)
        {
            Debug.Log("Player is Dead" + newhp);
            StopAllCoroutines();
            //true for gameover
            StopRPGBattle(true);
        }
        else
        {
            SaveData.instance.playerRPGHealth = newhp;
        }


    }

    private void DamageEnemy(float dmg, float mlp, bool debuff)
    {
        float hp = enemyHealth;
        float newhp;
        if (debuff)
        {
            newhp = hp - ((dmg * mlp) * 2);
        }
        else
        {
            newhp = hp - (dmg * mlp);
        }

        if (newhp <= 0)
        {
            Debug.Log("Enemy is Dead");
            StopAllCoroutines();
            //false for gameover
            StopRPGBattle(false);
        }
        else
        {
            enemyHealth = newhp;
        }
    }

    private void ChargeEnemy()
    {
        Debug.Log("eneyChargeCountdown: " + eneyChargeCountdown);
        if (eneyChargeCountdown == 0)
        {
            Debug.Log("Enemy Charge is over, Enemy Attacking");
            DamagePlayer(enemyBaseAttack, 2, playerGuard);
            enemyChargeIcon.SetActive(false);
            enemyCharge = false;
        }
        else
        {
            eneyChargeCountdown--;
        }
    }

    private void HealPlayer(float healPercentage, float energyPercentage)
    {
        float hp = SaveData.instance.playerRPGHealth;
        float mp = SaveData.instance.playerRPGEnergy;
        float totalHp = SaveData.instance.playerRPGTotalHealth;
        float totalMp = SaveData.instance.playerRPGTotalEnergy;
        float newhp = hp + CalculatePercentage(healPercentage, totalHp);// ((healPercentage * hp) / 100);
        float newEnergy = mp - CalculatePercentage(healPercentage, totalMp); //((energyPercentage * mp) / 100);
        //Debug.Log("Player health: " + hp);
        //Debug.Log("Heal for : " + ((healPercentage * hp) / 100) + " Mp: " + newEnergy + " New hp: " + newhp);

        if (hp == SaveData.instance.playerRPGTotalHealth)
        {
            InfoPanel.instance.TriggerInfoText("Health is full", Color.green);

        }
        else
        {
            if (newhp > SaveData.instance.playerRPGTotalHealth)
            {
                SaveData.instance.playerRPGHealth = SaveData.instance.playerRPGTotalHealth;
                SaveData.instance.playerRPGEnergy = newEnergy;
                Debug.Log("Player health: " + SaveData.instance.playerRPGHealth);

            }
            else
            {
                SaveData.instance.playerRPGHealth = newhp;
                SaveData.instance.playerRPGEnergy = newEnergy;
                Debug.Log("Player health: " + SaveData.instance.playerRPGHealth);

            }
        }


    }

    private void UseAbility(RPGPlayerAbilities ability)
    {
        float mp = SaveData.instance.playerRPGEnergy;
        float totalmp = SaveData.instance.playerRPGTotalHealth;
        float newEnergy;

        switch (ability)
        {
            case RPGPlayerAbilities.Heal:
                HealPlayer(playerHealingAbility, energyRequiredHeal);
                break;
            case RPGPlayerAbilities.Double:
                newEnergy = mp - CalculatePercentage(energyRequiredDouble, totalmp);
                SaveData.instance.playerRPGEnergy = newEnergy;
                DamageEnemy(SaveData.instance.playerRPGDamage, 2, enemyDebuff);
                break;
            case RPGPlayerAbilities.Debuff:
                newEnergy = mp - CalculatePercentage(energyRequiredDebuff, totalmp);
                SaveData.instance.playerRPGEnergy = newEnergy;
                enemyDebuffIcon.SetActive(true);
                enemyDebuff = true;
                break;
        }
    }

    private float CalculatePercentage(float percentage, float value)
    {
        return (percentage * value) / 100;
    }

    private void UseItem(RPGPlayerItems item)
    {
        switch (item)
        {
            case RPGPlayerItems.HealthBooster:
                if (SaveData.instance.playerRPGHealthBooster > 0)
                {
                    SaveData.instance.playerRPGHealthBooster--;
                    float hp = SaveData.instance.playerRPGHealth;
                    if ((hp + 50) > SaveData.instance.playerRPGTotalHealth)
                    {
                        SaveData.instance.playerRPGHealth = SaveData.instance.playerRPGTotalHealth;
                    }
                    else
                    {
                        SaveData.instance.playerRPGHealth += 50;
                    }
                    InfoPanel.instance.TriggerInfoText("Health Booster used, 50 health gained", Color.yellow);
                }
                break;
            case RPGPlayerItems.EnergyBooster:
                if (SaveData.instance.playerRPGEnergyBooster > 0)
                {
                    SaveData.instance.playerRPGEnergyBooster--;
                    float mp = SaveData.instance.playerRPGEnergy;
                    InfoPanel.instance.TriggerInfoText("Energy Booster used, 50 energy gained", Color.yellow);
                    if ((mp + 50) > SaveData.instance.playerRPGTotalEnergy)
                    {
                        SaveData.instance.playerRPGEnergy = SaveData.instance.playerRPGTotalHealth;
                    }
                    else
                    {
                        SaveData.instance.playerRPGEnergy += 50;
                    }
                }
                break;
            case RPGPlayerItems.DamageBooster:
                if (SaveData.instance.playerRPGDamageBooster > 0)
                {
                    InfoPanel.instance.TriggerInfoText("Damage Booster used, damage increased by 20 points", Color.yellow);
                    SaveData.instance.playerRPGDamageBooster--;
                    SaveData.instance.playerRPGDamage += 20;
                }
                break;
        }
        UpdateItemButton();
        UpdateAllInformation();
    }

    private void UpdateItemButton()
    {

        if (SaveData.instance.playerRPGHealthBooster <= 0)
        {
            healthBooster.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            healthBooster.interactable = false;
        }
        else
        {
            healthBooster.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            healthBooster.interactable = true;

        }

        if (SaveData.instance.playerRPGDamageBooster <= 0)
        {
            damageBooster.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            damageBooster.interactable = false;
        }
        else
        {
            damageBooster.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            damageBooster.interactable = true;

        }

        if (SaveData.instance.playerRPGEnergyBooster <= 0)
        {
            energyBooster.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            energyBooster.interactable = false;
        }
        else
        {
            energyBooster.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            energyBooster.interactable = true;

        }
    }

    private void UpdateAllBars()
    {
        enemyHealthBar.fillAmount = (float)(enemyHealth / totalEnemyHealth);
        //UnityEngine.Debug.Log("Enemy Health %: " + dmg / enemyHealth);
        float fillPercentage = SaveData.instance.playerRPGHealth / SaveData.instance.playerRPGTotalHealth;
        playerHealthBar.fillAmount = fillPercentage;
        //UnityEngine.Debug.Log("Player Health %: " + (totalPlayerHealth / SaveData.instance.playerRPGHealth) + "\nPlayer health: " + SaveData.instance.playerRPGHealth + "\n Total Player health: " + totalPlayerHealth);
        playerEnergyBar.fillAmount = SaveData.instance.playerRPGEnergy / SaveData.instance.playerRPGTotalEnergy;
        //UnityEngine.Debug.Log("Player Health %: " + totalPlayerHealth / SaveData.instance.playerRPGHealth + "Player health: " + SaveData.instance.playerRPGHealth);
    }

    private void UpdateInfoText()
    {
        enemyHealthText.text = "" + enemyHealth + " / " + totalEnemyHealth;
        playerHealthText.text = "" + SaveData.instance.playerRPGHealth + " / " + SaveData.instance.playerRPGTotalHealth;
        playerEnergyText.text = "" + SaveData.instance.playerRPGEnergy + " / " + SaveData.instance.playerRPGTotalEnergy;
        healthBoosterText.text = "" + SaveData.instance.playerRPGHealthBooster;
        energyBoosterText.text = "" + SaveData.instance.playerRPGEnergyBooster;
        damageBoosterText.text = "" + SaveData.instance.playerRPGDamageBooster;
    }

    private void UpdateAllInformation()
    {
        UpdateAllBars();
        UpdateInfoText();
    }


    #region OutsideTurnBase

    public void ToggleSpecialPanel()
    {
        if (toggleSpecial)
        {
            specialPanel.SetActive(true);
            toggleSpecial = false;
        }
        else
        {
            specialPanel.SetActive(false);
            toggleSpecial = true;
        }


    }

    public void ToggleItemsPanel()
    {
        if (toggleItems)
        {
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

    #endregion

    private void ResetRPGFight()
    {
        enemyHealth = totalEnemyHealth;
        enemyDebuff = false;
    }


    IEnumerator RPGGameOver(float wait)
    {
        yield return new WaitForSeconds(wait);
        RPGGameOverPanel.SetActive(true);

    }

    public void Restart()
    {
        SaveData.instance.playerRPGHealth = SaveData.instance.playerRPGTotalHealth / 2;
        SaveManager.saveManager_Instance.SaveGame();
        SceneManager.LoadScene(3);
    }

    public void StartRPGBattle(bool isBoss, float health, float dmg)
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerRPGUIControls>().DisableInputs();
        this.isBoss = isBoss;
        enemyHealth = health;
        enemyBaseAttack = dmg;
        totalEnemyHealth = enemyHealth;
        battleAnimator.SetTrigger("BattleStart");
        StartCoroutine(PlayBattleMusic());
        if (isBoss)
        {
            enemyType.text = "Boss";
        }
        else
        {
            enemyType.text = "Enemy";
        }
        StartCoroutine(ActiveBattlePanel());
        //RPGBattlePanel.SetActive(true);
        //UpdateInfoText();
        UpdateAllInformation();
        UpdateItemButton();
    }

    IEnumerator ActiveBattlePanel()
    {
        yield return new WaitForSeconds(1f);
        RPGBattlePanel.SetActive(true);
    }

    // index 3 on start , index 1 on stop
    IEnumerator PlayBattleMusic()
    {
        enemyEncounterSFX.Play();
        normalMusic.Stop();

        yield return new WaitForSeconds(enemyEncounterSFX.time);
        battleMusic.Play();
    }

    public void StopRPGBattle(bool gameOver)
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerRPGUIControls>().EnableInputs();
        StartCoroutine(DelayOnBattleEnd());
        //battleAnimator.ResetTrigger("StartAnim");
        // StartCoroutine(DelayOnAnimationStop());
        if (gameOver)
        {
            battleAnimator.SetTrigger("BattlePlayerLost");
            StartCoroutine(RPGGameOver(2));
        }
        else
        {
            if (isBoss)
            {
                ResetRPGFight();
                SceneManager.LoadScene(4);
            }
            else
            {
                ResetRPGFight();
            }
            battleAnimator.SetTrigger("BattlePlayerWon");

        }

        normalMusic.Play();
        battleMusic.Stop();
        enemyDebuff = false;
        enemyDebuffIcon.SetActive(false);
        enemyCharge = false;
        enemyChargeIcon.SetActive(false);
        enemyDebuffCountdown = 3;
        PlayerRPGUIControls.randomEnemyEncounter = false;
        CloseAllPanels();
    }

    IEnumerator DelayOnBattleEnd()
    {
        yield return new WaitForSeconds(1f);
        RPGBattlePanel.SetActive(false);
    }

    private void OnEnable()
    {

        PlayerRPGUIControls.onRandomEncounter += StartRPGBattle;
        RPGBoss.onBossEncounter += StartRPGBattle;

    }

    private void OnDisable()
    {
        PlayerRPGUIControls.onRandomEncounter -= StartRPGBattle;
        RPGBoss.onBossEncounter -= StartRPGBattle;
    }

}

public enum RPGEnemyType
{
    Boss,
    Enemy,
}

public enum RPGPlayerActions
{
    Attack,
    Guard,
    UseAbility,
    UseItem
}

public enum RPGPlayerAbilities
{
    Heal,
    Double,
    Debuff
}

public enum RPGPlayerItems
{
    HealthBooster,
    EnergyBooster,
    DamageBooster
}

public enum RPGEnemyActions
{
    Attack,
    Charge
}



/*
 *     public void UseHealthBooster()
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

 * 
 * 
 *         RPGPlayer.onPlayerAttack -= EnemyTakeDamage;
        RPGPlayer.onPlayerGuard -= UpdatePlayerGuardIcon;
        RPGPlayer.onPlayerGuard -= EnemyTakeDamage;
        RPGPlayer.onPlayerAbility -= PlayerActivatedAbility;
 * 
 * 
 *         RPGPlayer.onPlayerAttack += EnemyTakeDamage;
        RPGPlayer.onPlayerGuard += UpdatePlayerGuardIcon;
        RPGPlayer.onPlayerGuard += EnemyTakeDamage;
        RPGPlayer.onPlayerAbility += PlayerActivatedAbility;
 * 
 * 
                 //EnemyTakeDamage(SaveData.instance.playerRPGDamage * 2);
                float tempHealth = SaveData.instance.playerRPGEnergy; 
                float tempResultEnergy = tempHealth -= 50;
                if (tempResultEnergy < 0)
                {
                    No no !!!

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


 
 */