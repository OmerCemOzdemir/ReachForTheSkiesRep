using TMPro;
using UnityEngine;

public class RPGUIControls : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject repairPanel;
    [SerializeField] private GameObject itemCraterPanel;

    [SerializeField] private TextMeshProUGUI organicText;
    [SerializeField] private TextMeshProUGUI metalScrapText;
    [SerializeField] private TextMeshProUGUI chemicalText;

    public void CloseStations()
    {
        upgradePanel.SetActive(false);
        repairPanel.SetActive(false);
        itemCraterPanel.SetActive(false);
        ToolTipSystem.HideToolTip();
    }

    private void Start()
    {
        UpdateMaterialText();
    }

    public void UpdateMaterialText()
    {
        organicText.text = "" + SaveData.instance.organicMaterials;
        metalScrapText.text = "" + SaveData.instance.metalScrapMaterials;
        chemicalText.text = "" + SaveData.instance.chemicalMaterials;

    }


}
