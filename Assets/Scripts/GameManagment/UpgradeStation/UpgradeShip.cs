using TMPro;
using UnityEngine;

public class UpgradeShip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shipHealthText;
    [SerializeField] private TextMeshProUGUI shipDamageText;
    [SerializeField] private TextMeshProUGUI shipMultiShotText;
    [SerializeField] private TextMeshProUGUI shipFireRateText;

    [SerializeField] private TextMeshProUGUI organicText;
    [SerializeField] private TextMeshProUGUI metalScrapText;
    [SerializeField] private TextMeshProUGUI chemicalText;
    private RPGUIControls RPGUIControls;


    private void Start()
    {
        RPGUIControls = GetComponent<RPGUIControls>();
        UpdateMaterialText();
        UpdateShipStats();
    }

    public void UpgradeShipHealth()
    {
        if (SaveData.instance.metalScrapMaterials > 2 && SaveData.instance.chemicalMaterials > 1)
        {
            SaveData.instance.metalScrapMaterials -= 2;
            SaveData.instance.chemicalMaterials--;
            SaveData.instance.playerShipHealth += 10;
            SaveData.instance.playerShipTotalHealth += 10;
            RPGUIControls.UpdateMaterialText();
            UpdateShipStats();
            UpdateMaterialText();
        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Not enough Materials to upgrade", Color.red);
            Debug.Log("Not enough Materials to upgrade");
        }
    }

    public void UpgradeShipDamage()
    {
        if (SaveData.instance.metalScrapMaterials > 3 && SaveData.instance.chemicalMaterials > 2 && SaveData.instance.organicMaterials > 1)
        {
            SaveData.instance.metalScrapMaterials -= 3;
            SaveData.instance.chemicalMaterials -= 2;
            SaveData.instance.organicMaterials -= 1;
            SaveData.instance.playerShipDamage += 15;
            SaveData.instance.playerShipDamage += 15;
            RPGUIControls.UpdateMaterialText();
            UpdateShipStats();
            UpdateMaterialText();
        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Not enough Materials to upgrade", Color.red);

            Debug.Log("Not enough Materials to upgrade");
        }
    }

    public void UpgradeShipFireRate()
    {
        if (SaveData.instance.metalScrapMaterials > 5 && SaveData.instance.chemicalMaterials > 5)
        {
            SaveData.instance.metalScrapMaterials -= 5;
            SaveData.instance.chemicalMaterials -= 5;
            if (SaveData.instance.playerShipFireRate > 0.5f)
            {
                SaveData.instance.playerShipFireRate = 0.35f;
            }
            else if (SaveData.instance.playerShipFireRate == 0.35f)
            {
                SaveData.instance.playerShipFireRate = 0.2f;
            }

            RPGUIControls.UpdateMaterialText();
            UpdateShipStats();
            UpdateMaterialText();
        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Not enough Materials to upgrade", Color.red);

            Debug.Log("Not enough Materials to upgrade");
        }
    }

    public void UpgradeShipMultishot()
    {
        if (SaveData.instance.metalScrapMaterials > 15 && SaveData.instance.chemicalMaterials > 5 && SaveData.instance.organicMaterials > 5)
        {
            SaveData.instance.metalScrapMaterials -= 3;
            SaveData.instance.chemicalMaterials -= 2;
            SaveData.instance.organicMaterials -= 1;
            SaveData.instance.playerShipMultishot = true;
            RPGUIControls.UpdateMaterialText();
            UpdateShipStats();
            UpdateMaterialText();

        }
        else
        {

            InfoPanel.instance.TriggerInfoText("Not enough Materials to upgrade", Color.red);
            Debug.Log("Not enough Materials to upgrade");
        }
    }

    private void UpdateMaterialText()
    {
        organicText.text = "" + SaveData.instance.organicMaterials;
        metalScrapText.text = "" + SaveData.instance.metalScrapMaterials;
        chemicalText.text = "" + SaveData.instance.chemicalMaterials;

    }

    private void UpdateShipStats()
    {
        shipHealthText.text = "" + SaveData.instance.playerShipHealth + " / " + SaveData.instance.playerShipTotalHealth;
        shipDamageText.text = "" + SaveData.instance.playerShipDamage;
        if (SaveData.instance.playerShipMultishot)
        {
            shipMultiShotText.text = "On";
        }
        else
        {
            shipMultiShotText.text = "Off";
        }

        if (SaveData.instance.playerShipFireRate > 0.5f)
        {
            shipFireRateText.text = "Slow";
            shipFireRateText.color = Color.red;

        }
        else if (SaveData.instance.playerShipFireRate == 0.35f)
        {
            shipFireRateText.text = "Fast";
            shipFireRateText.color = Color.yellow;
        }
        else if (SaveData.instance.playerShipFireRate == 0.2f)
        {
            shipFireRateText.text = "Fastest";
            shipFireRateText.color = Color.green;
        }

    }


}
