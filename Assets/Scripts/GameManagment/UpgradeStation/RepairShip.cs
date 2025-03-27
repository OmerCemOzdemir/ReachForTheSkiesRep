using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairShip : MonoBehaviour
{
    //[SerializeField] float requiredAmountOfMetalPercentage;
    //[SerializeField] float requiredAmountOfChemicalPercentage;
    //I GOTTA DO MATH !! FOR NOW FOR EVERY 20 HP = 1 METAL
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI metalScraps;
    [SerializeField] private TextMeshProUGUI chemical;
    [SerializeField] private float metalScrapHealth;

    private RPGUIControls RPGUIControls;
    private void Start()
    {
        RPGUIControls = GetComponent<RPGUIControls>();
        healthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
        metalScraps.text = "" + SaveData.instance.metalScrapMaterials;
        chemical.text = "" + SaveData.instance.chemicalMaterials;

    }

    private void UpdateInformation()
    {
        healthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
        metalScraps.text = "" + SaveData.instance.metalScrapMaterials;
        chemical.text = "" + SaveData.instance.chemicalMaterials;
        RPGUIControls.UpdateMaterialText();
    }


    public void Repair()
    {

        float health = SaveData.instance.playerShipHealth;
        float totalHealth = SaveData.instance.playerShipTotalHealth;

        Debug.Log("Health Parametre: " + "\n TotalHealth: " + totalHealth
         + "\n Health: " + health + " Is Health Bigger: " + (totalHealth == health));

        if (totalHealth > health)
        {
            //100 - 20 = 80  //// 80 / 20 = 4 
            if (SaveData.instance.metalScrapMaterials > 1)
            {
                SaveData.instance.metalScrapMaterials -= 1;
                //SaveData.instance.chemicalMaterials--;

                float newHealth = SaveData.instance.playerShipHealth + metalScrapHealth;
                if (newHealth > SaveData.instance.playerShipTotalHealth)
                {
                    SaveData.instance.playerShipHealth = SaveData.instance.playerShipTotalHealth;
                }
                else
                {
                    SaveData.instance.playerShipHealth = newHealth;
                }
            }
            else
            {
                InfoPanel.instance.TriggerInfoText("Not enough Materials to repair", Color.red);
                //Debug.Log("Not enough Materials to repair");
            }
        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Health is already full", Color.green);

        }
        UpdateInformation();

    }

    public void RepairAll()
    {
        float health = SaveData.instance.playerShipHealth;
        float totalHealth = SaveData.instance.playerShipTotalHealth;

        if (totalHealth > health)
        {
            //100 - 20 = 80  //// 80 / 20 = 4 
            float metalScrapRequired = (totalHealth - health) / metalScrapHealth; //metalScrapHealth = 20 normally
            if (SaveData.instance.metalScrapMaterials > (int)metalScrapRequired)
            {
                SaveData.instance.metalScrapMaterials -= (int)metalScrapRequired;
                //SaveData.instance.chemicalMaterials--;
                SaveData.instance.playerShipHealth = SaveData.instance.playerShipTotalHealth;
                // SaveData.instance.playerShipHealth += 20;
            }
            else
            {
                InfoPanel.instance.TriggerInfoText("Not enough Materials to repair", Color.red);
            }

        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Health is already full", Color.green);

        }
        UpdateInformation();

    }

    public void RepairWithChemical()
    {
        float health = SaveData.instance.playerShipHealth;
        float totalHealth = SaveData.instance.playerShipTotalHealth;

        if (totalHealth > health)
        {
            //100 - 20 = 80  //// 80 / 20 = 4 
            if (SaveData.instance.metalScrapMaterials > 1 && SaveData.instance.chemicalMaterials > 1)
            {
                SaveData.instance.metalScrapMaterials -= 1;
                SaveData.instance.chemicalMaterials--;
                float newHealth = SaveData.instance.playerShipHealth + (metalScrapHealth * 2);
                if (newHealth > SaveData.instance.playerShipTotalHealth)
                {
                    SaveData.instance.playerShipHealth = SaveData.instance.playerShipTotalHealth;
                }
                else
                {
                    SaveData.instance.playerShipHealth = newHealth;
                }
            }
            else
            {
                InfoPanel.instance.TriggerInfoText("Not enough Materials to repair", Color.red);
                //Debug.Log("Not enough Materials to repair");
            }
        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Health is already full", Color.green);

        }
        UpdateInformation();

    }

}
/*
         if (SaveData.instance.metalScrapMaterials > 1)
        {
            SaveData.instance.metalScrapMaterials -= 1;
            //SaveData.instance.chemicalMaterials--;
            SaveData.instance.playerShipHealth += 20;
            UpdateInformation();
        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Not enough Materials to repair", Color.red);

            //Debug.Log("Not enough Materials to repair");
        }




     
 
 */