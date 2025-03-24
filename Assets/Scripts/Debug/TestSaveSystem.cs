using TMPro;
using UnityEngine;

public class TestSaveSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayInfo;


    public void SaveGame()
    {
        SaveManager.saveManager_Instance.SaveGame();

    }

    public void LoadGame()
    {

        SaveManager.saveManager_Instance.LoadGame();
        UpdateText();
    }


    public void WriteInfo()
    {

        SaveManager.saveManager_Instance.TestChange();

    }

    public void ResetInfo()
    {
        SaveManager.saveManager_Instance.TestReset();


    }


    private void UpdateText()
    {
        displayInfo.text = "Player Health:  " + SaveData.instance.playerShipHealth + " Player Damage: " + SaveData.instance.playerShipDamage;

    }

}
