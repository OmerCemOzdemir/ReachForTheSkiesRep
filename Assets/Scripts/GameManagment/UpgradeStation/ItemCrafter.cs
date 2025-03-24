using TMPro;
using UnityEngine;

public class ItemCrafter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI organicText;
    [SerializeField] private TextMeshProUGUI chemicalText;
    [SerializeField] private TextMeshProUGUI healthBoosterText;
    [SerializeField] private TextMeshProUGUI damageBoosterText;
    [SerializeField] private TextMeshProUGUI energyBoosterText;

    private RPGUIControls RPGUIControls;


    private void Start()
    {
        RPGUIControls = GetComponent<RPGUIControls>();
        UpdateTexts();
    }

    public void CreateHealthBooster()
    {
        if (SaveData.instance.chemicalMaterials > 1 && SaveData.instance.organicMaterials > 2)
        {
            SaveData.instance.chemicalMaterials -= 1;
            SaveData.instance.organicMaterials -= 2;
            SaveData.instance.playerRPGHealthBooster++;
            RPGUIControls.UpdateMaterialText();
            UpdateTexts();

        }
        else
        {

            InfoPanel.instance.TriggerInfoText("Not enough Materials to create", Color.red);
            Debug.Log("Not enough Materials to create");
        }
    }

    public void CreateDamageBooster()
    {
        if (SaveData.instance.chemicalMaterials > 2 && SaveData.instance.organicMaterials > 1)
        {
            SaveData.instance.chemicalMaterials -= 1;
            SaveData.instance.organicMaterials -= 2;
            SaveData.instance.playerRPGDamageBooster++; 
            RPGUIControls.UpdateMaterialText();
            UpdateTexts();

        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Not enough Materials to create", Color.red);
            Debug.Log("Not enough Materials to create");
        }

    }

    public void CreateEnergyBooster()
    {
        if (SaveData.instance.chemicalMaterials > 2 && SaveData.instance.organicMaterials > 2)
        {
            SaveData.instance.chemicalMaterials -= 2;
            SaveData.instance.organicMaterials -= 2;
            SaveData.instance.playerRPGEnergyBooster++;
            RPGUIControls.UpdateMaterialText();
            UpdateTexts();

        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Not enough Materials to create", Color.red);
            Debug.Log("Not enough Materials to create");
        }

    }

    private void UpdateTexts()
    {
        organicText.text = "" + SaveData.instance.organicMaterials;
        chemicalText.text = "" + SaveData.instance.chemicalMaterials;
        healthBoosterText.text = "" + SaveData.instance.playerRPGHealthBooster;
        damageBoosterText.text = "" + SaveData.instance.playerRPGDamageBooster;
        energyBoosterText.text = "" + SaveData.instance.playerRPGEnergyBooster;

    }



}
