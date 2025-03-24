using TMPro;
using UnityEngine;

public class GameLoot : MonoBehaviour
{
    [SerializeField] private GameObject LootPanel;
    [SerializeField] private GameObject LootContent;

    /*
     HealthBooster = 0
     DamageBooster  = 1
     EnergyBooster = 2

     Organic = 3
     Metal = 4
     Chemical =5
     */


    public void CloseLootPanel()
    {
        LootPanel.SetActive(false);
    }


    private void OpenLootPanel(int[] temp)
    {
        LootPanel.SetActive(true);
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] != 0)
            {
                LootContent.transform.GetChild(i).gameObject.SetActive(true);
                LootContent.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + temp[i];
                switch (i)
                {
                    case 0:
                        //HealthBooster
                        SaveData.instance.playerRPGHealthBooster += temp[i];
                        break;
                    case 1:
                        SaveData.instance.playerRPGDamageBooster += temp[i];
                        //DamageBooster
                        break;
                    case 2:
                        SaveData.instance.playerRPGEnergyBooster += temp[i];
                        //EnergyBooster
                        break;
                    case 3:
                        SaveData.instance.organicMaterials += temp[i];
                        //Organic
                        break;
                    case 4:
                        SaveData.instance.metalScrapMaterials += temp[i];

                        //Metal
                        break;
                    case 5:
                        SaveData.instance.chemicalMaterials += temp[i];

                        //Chemical
                        break;
                }
            }
        }


    }


    private void OnEnable()
    {
        PlayerRPGUIControls.onLootDrop += OpenLootPanel;
    }

    private void OnDisable()
    {
        PlayerRPGUIControls.onLootDrop -= OpenLootPanel;

    }
}
