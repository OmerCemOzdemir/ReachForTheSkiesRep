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
        RPGFightManagerCopy = GetComponent<RPGFightManager>();
    }

    private void Start()
    {
        UpdatePlayerStats();
    }


    public void UpdatePlayerStats()
    {
        playerHealthText.text = "" + SaveData.instance.playerRPGHealth + " / " + RPGFightManagerCopy.GetTotalPlayerHealth();
        playerEnergyText.text = "" + SaveData.instance.playerRPGEnergy + " / " + RPGFightManagerCopy.GetTotalPlayerEnergy();
        playerEnergyBar.fillAmount = SaveData.instance.playerRPGEnergy / RPGFightManagerCopy.GetTotalPlayerEnergy();
        playerHealthBar.fillAmount = SaveData.instance.playerRPGHealth / RPGFightManagerCopy.GetTotalPlayerHealth();
        UpdateMaterialText();
        UpdateTexts();
        Debug.Log("Player Stats are updated: " + SaveData.instance.playerRPGHealth + " / " + RPGFightManagerCopy.GetTotalPlayerHealth());
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


    public void OpenDebugMenu()
    {
        debugMenuRPG.SetActive(true);
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
