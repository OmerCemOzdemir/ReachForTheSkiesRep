using TMPro;
using UnityEngine;

public class DebugRPG : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI printSaveText;

    public void DebugChemicals()
    {
        SaveData.instance.chemicalMaterials += 10;

    }

    public void DebugOrganics()
    {
        SaveData.instance.organicMaterials += 10;

    }
    public void DebugMetals()
    {
        SaveData.instance.metalScrapMaterials += 10;

    }

    public void DebugBoosterHeal()
    {
        SaveData.instance.playerRPGHealthBooster += 10;
    }
    public void DebugBoosterDmg()
    {
        SaveData.instance.playerRPGDamageBooster += 10;

    }
    public void DebugBoosterEnergy()
    {
        SaveData.instance.playerRPGEnergyBooster += 10;

    }

    public void DebugInfoText()
    {
        InfoPanel.instance.TriggerInfoText("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla facilisi. ", Color.gray);
    }


    public void PrintSave()
    {
        printSaveText.text = "Ship Stats: " + "\n Ship Health: " + SaveData.instance.playerShipHealth + "/" + SaveData.instance.playerShipTotalHealth +
            "\n Ship Damage: " + SaveData.instance.playerShipDamage +
            "\n Ship Move Speed: " + SaveData.instance.playerShipMoveSpeed +
            "\n Ship Projectile Speed: " + SaveData.instance.playerShipProjectileSpeed +
            "\n Ship MultiShot: " + SaveData.instance.playerShipMultishot +
            "\n Ship Game Level: " + SaveData.instance.gameShipLevel +
            "\n Ship Game Stage Timer: " + SaveData.instance.gameShipStageTimer +
            "\n RPG Stats: " +
            "\n RPG Player Health: " + SaveData.instance.playerRPGHealth +
            "\n RPG Player Damage: " + SaveData.instance.playerRPGDamage +
            "\n RPG Player Energy: " + SaveData.instance.playerRPGEnergy +
            "\n RPG Chemical Material: " + SaveData.instance.chemicalMaterials +
            "\n RPG MetalScrap Material: " + SaveData.instance.metalScrapMaterials +
            "\n RPG Organic Material: " + SaveData.instance.organicMaterials +
            "\n RPG HealthBoosters: " + SaveData.instance.playerRPGHealthBooster +
            "\n RPG DamageBoosters: " + SaveData.instance.playerRPGDamageBooster +
            "\n RPG EnergyBoosters: " + SaveData.instance.playerRPGEnergyBooster +
            "\n New Game: " + SaveData.instance.newGame +
            "\n RPG New Game: " + SaveData.instance.RPGnewGame;

    }



}
