using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairShip : MonoBehaviour
{
   // [SerializeField] float requiredAmountOfMetalPercentage;
    //[SerializeField] float requiredAmountOfChemicalPercentage;
    // I GOTTA DO MATH !! FOR NOW FOR EVERY 20 HP = 1 METAL
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI metalScraps;
    [SerializeField] private TextMeshProUGUI chemical;

    private RPGUIControls RPGUIControls;
    private void Start()
    {
        RPGUIControls = GetComponent<RPGUIControls>();
        healthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
        metalScraps.text = "" + SaveData.instance.metalScrapMaterials;
        chemical.text = "" + SaveData.instance.chemicalMaterials;

    }


    public void Repair()
    {
        if (SaveData.instance.metalScrapMaterials > 1 )
        {
            SaveData.instance.metalScrapMaterials -= 1;
            //SaveData.instance.chemicalMaterials--;
            SaveData.instance.playerShipHealth += 20;
            healthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
            metalScraps.text = "" + SaveData.instance.metalScrapMaterials;
            chemical.text = "" + SaveData.instance.chemicalMaterials;
            RPGUIControls.UpdateMaterialText();
        }
        else
        {
            InfoPanel.instance.TriggerInfoText("Not enough Materials to repair", Color.red);

            //Debug.Log("Not enough Materials to repair");
        }
    }

}
