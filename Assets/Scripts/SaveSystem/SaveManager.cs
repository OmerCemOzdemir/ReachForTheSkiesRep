using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : ScriptableObject
{

    private static SaveManager _saveManager_Instance;

    public static SaveManager saveManager_Instance
    {
        get
        {

            if (_saveManager_Instance == null)
            {
                _saveManager_Instance = ScriptableObject.CreateInstance<SaveManager>();
            }
            return _saveManager_Instance;
        }

    }


    public void SaveGame()
    {

        string json = JsonUtility.ToJson(SaveData.instance, true);
        File.WriteAllText(Application.dataPath + "/SaveData.json", json);
       // Debug.Log("Game Saved");

    }

    public void LoadGame()
    {
        string json = File.ReadAllText(Application.dataPath + "/SaveData.json");
        SaveData saveData = new SaveData();
        saveData = JsonUtility.FromJson<SaveData>(json);

        SaveData.instance.playerShipHealth = saveData.playerShipHealth;
        SaveData.instance.playerShipDamage = saveData.playerShipDamage;
        SaveData.instance.playerShipMoveSpeed = saveData.playerShipMoveSpeed;
        SaveData.instance.playerShipProjectileSpeed = saveData.playerShipProjectileSpeed;
        SaveData.instance.organicMaterials = saveData.organicMaterials;
        SaveData.instance.metalScrapMaterials = saveData.metalScrapMaterials;
        SaveData.instance.chemicalMaterials = saveData.chemicalMaterials;
        SaveData.instance.newGame = saveData.newGame;
        SaveData.instance.gameShipLevel = saveData.gameShipLevel;
        SaveData.instance.gameShipStageTimer = saveData.gameShipStageTimer;
        SaveData.instance.playerShipTotalHealth = saveData.playerShipTotalHealth;
        SaveData.instance.playerShipMultishot = saveData.playerShipMultishot;
        SaveData.instance.playerRPGHealth = saveData.playerRPGHealth;
        SaveData.instance.playerRPGDamage = saveData.playerRPGDamage;
        SaveData.instance.playerRPGEnergy = saveData.playerRPGEnergy;
        SaveData.instance.playerShipFireRate = saveData.playerShipFireRate;

        SaveData.instance.playerRPGHealthBooster = saveData.playerRPGHealthBooster;
        SaveData.instance.playerRPGDamageBooster = saveData.playerRPGDamageBooster;
        SaveData.instance.playerRPGEnergyBooster = saveData.playerRPGEnergyBooster;
        SaveData.instance.RPGnewGame = saveData.RPGnewGame;
        SaveData.instance.gameMasterVolume = saveData.gameMasterVolume;
        SaveData.instance.gameMusicVolume = saveData.gameMusicVolume;
        SaveData.instance.gameSFXVolume = saveData.gameSFXVolume;

    }

    public void TestChange()
    {
        SaveData.instance.playerShipHealth = 100;
        SaveData.instance.playerShipDamage = 20;

    }


    public void TestReset()
    {
        SaveData.instance.playerShipHealth = 0;
        SaveData.instance.playerShipDamage = 0;

    }

}

/*
//save game
    string json = JsonUtility.ToJson(data, true);
File.WriteAllText(Application.dataPath + "/SaveData.json", json);



//load game
 string json = File.ReadAllText(Application.dataPath + "/SaveData.json");
    data = JsonUtility.FromJson<SaveData>(json);
 */

