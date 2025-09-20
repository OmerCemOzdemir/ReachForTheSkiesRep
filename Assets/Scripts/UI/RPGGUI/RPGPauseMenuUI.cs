using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RPGPauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject shipItemsPanel;
    [SerializeField] private GameObject playerItemsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject debugMenuRPG;

    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI playerEnergyText;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerEnergyBar;

    [SerializeField] private TextMeshProUGUI healthBoosterText;
    [SerializeField] private TextMeshProUGUI damageBoosterText;
    [SerializeField] private TextMeshProUGUI energyBoosterText;

    private RPGFightManager RPGFightManagerCopy;

    [SerializeField] private TextMeshProUGUI organicText;
    [SerializeField] private TextMeshProUGUI metalScrapText;
    [SerializeField] private TextMeshProUGUI chemicalText;





    private void Awake()
    {
        RPGFightManagerCopy = FindAnyObjectByType<RPGFightManager>();
    }

    private void Start()
    {
        UpdatePlayerStats();
    }

    public void UpdatePlayerStats()
    {
        playerHealthText.text = "" + SaveData.instance.playerRPGHealth + " / " + SaveData.instance.playerRPGTotalEnergy;
        playerEnergyText.text = "" + SaveData.instance.playerRPGEnergy + " / " + SaveData.instance.playerRPGTotalHealth;
        playerEnergyBar.fillAmount = SaveData.instance.playerRPGEnergy / SaveData.instance.playerRPGTotalEnergy;
        playerHealthBar.fillAmount = SaveData.instance.playerRPGHealth / SaveData.instance.playerRPGTotalHealth;
        UpdateMaterialText();
        UpdateTexts();
        // Debug.Log("Player Stats are updated: " + SaveData.instance.playerRPGHealth + " / " + RPGFightManagerCopy.GetTotalPlayerHealth());
    }

    public void UpdateMaterialText()
    {
        organicText.text = "" + SaveData.instance.organicMaterials;
        metalScrapText.text = "" + SaveData.instance.metalScrapMaterials;
        chemicalText.text = "" + SaveData.instance.chemicalMaterials;

    }


    private void UpdateTexts()
    {

        healthBoosterText.text = "" + SaveData.instance.playerRPGHealthBooster;
        damageBoosterText.text = "" + SaveData.instance.playerRPGDamageBooster;
        energyBoosterText.text = "" + SaveData.instance.playerRPGEnergyBooster;

    }

    public void UseHealSkill(float healPercentage)
    {
        float hp = SaveData.instance.playerRPGHealth;
        float mp = SaveData.instance.playerRPGEnergy;
        float newhp = hp + ((healPercentage * hp) / 100);
        float newEnergy = mp - (((healPercentage * hp) / 100) / 2);
        Debug.Log("Player health: " + hp);
        Debug.Log("Heal for : " + ((healPercentage * hp) / 100) + " Mp: " + newEnergy + " New hp: " + newhp);

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
        UpdatePlayerStats();
    }


    public void OpenDebugMenu()
    {
        debugMenuRPG.SetActive(true);
    }

    public void CloseDebugMenu()
    {
        debugMenuRPG.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void OpenSkills()
    {
        skillsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
    }


    public void OpenPlayerItemsPanel()
    {
        shipItemsPanel.SetActive(false);
        playerItemsPanel.SetActive(true);
    }

    public void OpenShipItemsPanel()
    {
        shipItemsPanel.SetActive(true);
        playerItemsPanel.SetActive(false);

    }

    private void CloseAllPanels()
    {
        shipItemsPanel.SetActive(false);
        playerItemsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        debugMenuRPG.SetActive(false);
        skillsPanel.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdatePlayerStats();
        }
    }

    public void Return()
    {
        CloseAllPanels();
        shipItemsPanel.SetActive(true);

    }

    private void OnEnable()
    {
        RPGFightManager.onBattleProgress += UpdatePlayerStats;
        PlayerRPGUIControls.onEscapePressed += UpdatePlayerStats;
    }

    private void OnDisable()
    {
        RPGFightManager.onBattleProgress -= UpdatePlayerStats;
        PlayerRPGUIControls.onEscapePressed -= UpdatePlayerStats;

    }

}
