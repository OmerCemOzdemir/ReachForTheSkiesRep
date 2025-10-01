using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipGUI : MonoBehaviour
{
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private PlayerShipController playerShipController;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private ShipGameManager shipGameManager;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI metalText;
    [SerializeField] private TextMeshProUGUI chemText;
    [SerializeField] private TextMeshProUGUI bioText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverCountdownText;

    [SerializeField] private GameObject settingsPanel;


    private int currentStage = 0;
    private Vector3 stageTextPosition;
    public static event Action onNewStageStart;
    public static event Action onGameStart;


    private void Update()
    {
        timerText.text = "" + (int)shipGameManager.timer;
        UpdateMatText();
    }


    private void Start()
    {
        //UpdatePlayerHealthBar();
        playerHealthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
        stageTextPosition = stageText.transform.position;
        StartCoroutine(FirstStageLerp());
        //UpdateMatText();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void ReturnButton()
    {
        settingsPanel.SetActive(false);

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void UpdateStageTextPosition(int stage)
    {
        StartCoroutine(StartStageLerp());
    }

    IEnumerator FirstStageLerp()
    {
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint();
        yield return new WaitForSeconds(3f);
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint();
        yield return new WaitForSeconds(7f);
        onGameStart?.Invoke();
    }


    IEnumerator StartStageLerp()
    {
        EnemySpawner.endEnemySpawn = true;
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint();
        yield return new WaitForSeconds(3f);
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint();
        yield return new WaitForSeconds(7f);
        EnemySpawner.endEnemySpawn = false;
        onNewStageStart?.Invoke();
    }

    private void UpdateStageText(int stage)
    {
        currentStage++;
        switch (stage)
        {
            case 1:
                stageText.text = "Stage I";
                break;
            case 2:
                stageText.text = "Stage II";
                break;
            case 3:
                stageText.text = "Stage III";
                break;
        }
        //Debug.Log("Current Stage: " + currentStage);
    }

    private void UpdatePlayerHealthBar()
    {
        playerHealthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
        //float dmg = (float)playerShipController.GetDamageTaken() / SaveData.instance.playerShipHealth;
        //Debug.Log("health bar minus " + dmg);
    }

    private void UpdateMatText()
    {
        metalText.text = SaveData.instance.metalScrapMaterials.ToString();
        chemText.text = SaveData.instance.chemicalMaterials.ToString();
        bioText.text = SaveData.instance.organicMaterials.ToString();

    }

    private void ActivateGameOverSequence()
    {
        gameOverPanel.SetActive(true);
        //StartCoroutine(PlayerRespawnDelay(wait));

    }


    IEnumerator PlayerRespawnDelay(float wait)
    {
        while (wait > 0)
        {
            //Debug.Log("Time remaining: " + wait);
            gameOverCountdownText.text = "" + (int)wait;
            yield return new WaitForSeconds(1f);
            wait--;
        }

    }


    private void OnEnable()
    {
        PlayerShipController.OnPlayerUIUpdate += UpdatePlayerHealthBar;
        PlayerShipController.OnPlayerRespawn += ActivateGameOverSequence;
        ShipGameManager.onStageClear += UpdateStageText;
        ShipGameManager.onStageClear += UpdateStageTextPosition;

    }

    private void OnDisable()
    {
        PlayerShipController.OnPlayerUIUpdate -= UpdatePlayerHealthBar;
        PlayerShipController.OnPlayerRespawn -= ActivateGameOverSequence;
        ShipGameManager.onStageClear -= UpdateStageText;
        ShipGameManager.onStageClear -= UpdateStageTextPosition;
    }

}
/*
         if (SaveData.instance.newGame)
        {
            playerHealthBar.fillAmount = 1;
        }
        else
        {
            playerHealthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
        }
 
 */