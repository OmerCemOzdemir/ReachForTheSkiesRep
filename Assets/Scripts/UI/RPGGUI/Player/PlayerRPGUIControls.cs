using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerRPGUIControls : MonoBehaviour
{
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject repairPanel;
    [SerializeField] private GameObject itemCraterPanel;
    [SerializeField] private GameObject pauseMenuPanel;

    [SerializeField] private Material repairOutlineMat;
    [SerializeField] private Material upgradeOutlineMat;
    [SerializeField] private Material craftOutlineMat;



    private PlayerInput inputActionUI;
    private InputAction interact;
    private InputAction escape;

    private string stationType;
    //private bool playerInteracted = false;
    private bool toggleEscape = false;
    private GameObject tempLootChest;
    public static bool randomEnemyEncounter = false;

    public static event Action<bool, float, float> onRandomEncounter;
    public static event Action<int[]> onLootDrop;
    public static event Action onEscapePressed;


    private void Awake()
    {
        inputActionUI = new PlayerInput();
        interact = inputActionUI.PlayerRPG.Interact;
        escape = inputActionUI.PlayerRPG.Escape;

    }

    private void OnEnable()
    {
        interact.Enable();
        escape.Enable();
        interact.performed += PlayerInteraction;
        escape.performed += OpenPauseMenu;

    }

    private void OnDisable()
    {
        interact.Disable();
        escape.Disable();
        interact.performed -= PlayerInteraction;
        escape.performed -= OpenPauseMenu;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Upgrade"))
        {
            stationType = "Upgrade";
            interactText.SetActive(true);
            ChangeUpgradeMat();
        }

        if (collision.CompareTag("Repair"))
        {
            stationType = "Repair";
            interactText.SetActive(true);
            ChangeRepairMat();
        }

        if (collision.CompareTag("ItemCrafter"))
        {
            stationType = "ItemCrafter";
            interactText.SetActive(true);
            ChangeCraftMat();
        }


        if (collision.CompareTag("LootChest"))
        {
            stationType = "LootChest";
            interactText.SetActive(true);
            tempLootChest = collision.gameObject;
        }

        if (collision.CompareTag("DangerArea"))
        {
            //ActivateRandomzer(collision.gameObject);
            //StartCoroutine(RandomEnemyCycle(collision.gameObject));
            ActivateRandomzer(collision.gameObject);

            Debug.Log("Player is in Danger Zone");
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Upgrade"))
        {
            stationType = "None";
            interactText.SetActive(false);
            ChangeMatNormal();
        }

        if (collision.CompareTag("Repair"))
        {
            stationType = "None";
            interactText.SetActive(false);
            ChangeMatNormal();
        }

        if (collision.CompareTag("ItemCrafter"))
        {
            stationType = "None";
            interactText.SetActive(false);
            ChangeMatNormal();
        }

        if (collision.CompareTag("LootChest"))
        {
            stationType = "None";
            interactText.SetActive(false);

        }

        if (collision.CompareTag("DangerArea"))
        {
            //ActivateRandomzer(collision.gameObject);
            //StopCoroutine(RandomEnemyCycle(collision.gameObject));
            ActivateRandomzer(collision.gameObject);

            Debug.Log("Player is out of Danger Zone");
        }

    }


    private void ChangeRepairMat()
    {
        
        repairOutlineMat.SetColor("_OutlineColor", new Color(0.1569983f, 1, 0));
        repairOutlineMat.SetColor("_OutlineColor2", new Color(0, 0.7353768f, 1));
    }
    private void ChangeUpgradeMat()
    {
        //Debug.Log("mat changed");
        upgradeOutlineMat.SetColor("_OutlineColor", new Color(0.1569983f, 1, 0));
        upgradeOutlineMat.SetColor("_OutlineColor2", new Color(0, 0.7353768f, 1));
    }
    private void ChangeCraftMat()
    {
        craftOutlineMat.SetColor("_OutlineColor", new Color(0.1569983f, 1, 0));
        craftOutlineMat.SetColor("_OutlineColor2", new Color(0, 0.7353768f, 1));
    }

    private void ChangeMatNormal()
    {
        repairOutlineMat.SetColor("_OutlineColor", new Color(0, 0, 0));
        repairOutlineMat.SetColor("_OutlineColor2", new Color(0, 0, 0));
        upgradeOutlineMat.SetColor("_OutlineColor", new Color(0, 0, 0));
        upgradeOutlineMat.SetColor("_OutlineColor2", new Color(0, 0, 0));
        craftOutlineMat.SetColor("_OutlineColor", new Color(0, 0, 0));
        craftOutlineMat.SetColor("_OutlineColor2", new Color(0, 0, 0));
    }


    IEnumerator RandomEnemyCycle(GameObject enemy)
    {
        while (true)
        {
            if (!randomEnemyEncounter)
            {
                ActivateRandomzer(enemy);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }


    private void PlayerInteraction(InputAction.CallbackContext context)
    {
        //playerInteracted = true;
        interactText.SetActive(false);
        
        switch (stationType)
        {
            case "Upgrade":
                upgradePanel.SetActive(true);
                randomEnemyEncounter = true;
                break;
            case "Repair":
                repairPanel.SetActive(true);
                randomEnemyEncounter = true;
                break;
            case "ItemCrafter":
                itemCraterPanel.SetActive(true);
                randomEnemyEncounter = true;
                break;
            case "LootChest":
                //int[] temp = { 1, 0, 2, 0, 4, 0 };
                onLootDrop?.Invoke(tempLootChest.GetComponent<LootChest>().GameLoot);
                Destroy(tempLootChest);
                tempLootChest = null;
                break;
            case "None":
                break;

        }

    }


    private void OpenPauseMenu(InputAction.CallbackContext context)
    {
        Debug.Log("PressedEscape");
        onEscapePressed?.Invoke();
        if (toggleEscape)
        {
            pauseMenuPanel.SetActive(false);
            toggleEscape = false;
        }
        else
        {
            pauseMenuPanel.SetActive(true);
            toggleEscape = true;

        }
    }

    private void ActivateRandomzer(GameObject enemy)
    {
        float rand = UnityEngine.Random.Range(1, 50);

        if (rand == 20)
        {
            randomEnemyEncounter = true;
            float health = enemy.GetComponent<DangerZone>().EnemyHealth;
            float dmg = enemy.GetComponent<DangerZone>().EnemyDamage;
            onRandomEncounter?.Invoke(false, health, dmg);
        }

    }


}
