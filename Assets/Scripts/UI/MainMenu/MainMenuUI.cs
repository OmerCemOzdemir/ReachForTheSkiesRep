using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject continueButton;
    private bool askPlayerForConfirmation;
    [SerializeField] private GameObject confirmPanel;
    private void Start()
    {
        SaveManager.saveManager_Instance.LoadGame();

        if (SaveData.instance.newGame)
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
            askPlayerForConfirmation = true;
        }
    }


    public void NewGame()
    {
        if (askPlayerForConfirmation)
        {
            confirmPanel.SetActive(true);
        }
        else
        {
            //Ship Stats:  
            SaveData.instance.playerShipHealth = 0;
            SaveData.instance.playerShipDamage = 0;
            SaveData.instance.playerShipMoveSpeed = 0;
            SaveData.instance.playerShipProjectileSpeed = 0;
            SaveData.instance.playerShipMultishot = false;
            //Materials:
            SaveData.instance.chemicalMaterials = 0;
            SaveData.instance.organicMaterials = 0;
            SaveData.instance.metalScrapMaterials = 0;
            //Game Stages:
            SaveData.instance.newGame = true;
            SaveData.instance.RPGnewGame = true;
            SaveData.instance.gameShipLevel = 0;
            SaveData.instance.gameShipStageTimer = 0;
            //RPG Stats:        
            SaveData.instance.playerRPGHealth = 0;
            SaveData.instance.playerRPGDamage = 0;
            SaveData.instance.playerRPGEnergy = 0;
            SaveData.instance.playerRPGHealthBooster = 0;
            SaveData.instance.playerRPGDamageBooster = 0;
            SaveData.instance.playerRPGEnergyBooster = 0;
            //Settings:
            SaveData.instance.gameMasterVolume = 0;
            SaveData.instance.gameMusicVolume = 0;
            SaveData.instance.gameSFXVolume = 0;

            SaveManager.saveManager_Instance.SaveGame();
            SceneManager.LoadScene(1);
        }

    }

    public void ConfirmNewGame()
    {
        confirmPanel.SetActive(false);
        askPlayerForConfirmation = false;
        NewGame();
    }

    public void DeclineNewGame()
    {
        confirmPanel.SetActive(false);

    }

    public void ContinueGame()
    {
        SaveManager.saveManager_Instance.LoadGame();
        SceneManager.LoadScene(1);
    }


    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
