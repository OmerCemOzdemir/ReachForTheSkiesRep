using UnityEngine;

public class LootChest : MonoBehaviour
{
    [Header("     HealthBooster = 0\r\n     DamageBooster  = 1\r\n     EnergyBooster = 2\r\n\r\n     Organic = 3\r\n     Metal = 4\r\n     Chemical = 5")]
    [SerializeField] private int[] gameLoot; 

    public int[] GameLoot { get => gameLoot; set => gameLoot = value; }
}
