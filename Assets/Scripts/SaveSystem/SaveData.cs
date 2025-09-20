[System.Serializable]
public class SaveData
{

    private static SaveData _instance;

    public static SaveData instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveData();

            }
            return _instance;

        }
        set
        {
            _instance = value;
        }
    }


    public void SetGameData(SaveData newData)
    {
        _instance = newData;
    }

    public bool newGame = true;

    //Player Ship Stats:
    public float playerShipHealth;
    public float playerShipDamage;
    public float playerShipMoveSpeed;
    public float playerShipProjectileSpeed;
    public float playerShipTotalHealth;
    public bool playerShipMultishot;
    public float playerShipFireRate;

    //Player RPGStats: 
    public float playerRPGHealth;
    public float playerRPGDamage;
    public float playerRPGEnergy;
    public float playerRPGTotalHealth;
    public float playerRPGTotalDamage;
    public float playerRPGTotalEnergy;
    public float playerRPGHealthBooster;
    public float playerRPGDamageBooster;
    public float playerRPGEnergyBooster;

    public bool RPGnewGame = true;

    // Resources:
    public int organicMaterials;
    public int metalScrapMaterials;
    public int chemicalMaterials;

    //Game Stages:
    public int gameShipLevel;
    public float gameShipStageTimer;

    //Settings
    public float gameMusicVolume;
    public float gameSFXVolume;
    public float gameMasterVolume;


}
