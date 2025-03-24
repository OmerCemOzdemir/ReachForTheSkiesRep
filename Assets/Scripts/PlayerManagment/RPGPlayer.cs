using System;
using UnityEngine;
using UnityEngine.UI;

public class RPGPlayer : MonoBehaviour
{

    [Header("RPG Player Info: ")]
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerAttack;
    [SerializeField] private float playerEnergy;

    private void Start()
    {


        if (SaveData.instance.RPGnewGame)
        {
            SaveData.instance.playerRPGHealth = playerHealth;
            SaveData.instance.playerRPGDamage = playerAttack;
            SaveData.instance.playerRPGEnergy = PlayerEnergy;
            SaveData.instance.RPGnewGame = false;
        }
        else
        {
            playerHealth = SaveData.instance.playerRPGHealth;
            playerAttack = SaveData.instance.playerRPGDamage;
            PlayerEnergy = SaveData.instance.playerRPGEnergy;
        }
    }

    public float PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public float PlayerBaseAttack { get => playerAttack; set => playerAttack = value; }
    public float PlayerEnergy { get => playerEnergy; set => playerEnergy = value; }

    public static event Action<float> onPlayerAttack;
    public static event Action<float> onPlayerGuard;
    public static event Action<int> onPlayerAbility;



    public void Attack()
    {
        onPlayerAttack?.Invoke(playerAttack);
    }


    public void Guard()
    {
        onPlayerGuard?.Invoke(0);
    }


    public void Heal()
    {
        onPlayerAbility?.Invoke(1);

    }

    public void DoubleAttack()
    {
        onPlayerAbility?.Invoke(2);

    }


    public void DebuffEnemy()
    {
        onPlayerAbility?.Invoke(3);

    }


}

/*
 
 
 
/=-----------------------------------------------------

     
/=-----------------------------------------------------


 */