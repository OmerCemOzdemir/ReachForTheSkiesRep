using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    }


    private void Start()
    {
        //UpdatePlayerHealthBar();
        playerHealthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;
        stageTextPosition = stageText.transform.position;
        StartCoroutine(FirstStageLerp());
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

    private void UpdateStageTextPosition()
    {
        StartCoroutine(StartStageLerp());
    }

    IEnumerator FirstStageLerp()
    {
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint();
        yield return new WaitForSeconds(3f);
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint2();
        yield return new WaitForSeconds(7f);
        onGameStart?.Invoke();
    }


    IEnumerator StartStageLerp()
    {
        EnemySpawner.endEnemySpawn = true;
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint();
        yield return new WaitForSeconds(3f);
        stageText.gameObject.GetComponent<LerpObject>().LerpObjectToPoint2();
        yield return new WaitForSeconds(7f);
        EnemySpawner.endEnemySpawn = false;
        onNewStageStart?.Invoke();
    }

    private void UpdateStageText()
    {
        currentStage++;
        switch (currentStage)
        {
            case 0:
                stageText.text = "Stage I";
                break;
            case 1:
                stageText.text = "Stage II";
                break;
            case 2:
                stageText.text = "Stage III";
                break;
        }
        //Debug.Log("Current Stage: " + currentStage);
    }

    private void UpdatePlayerHealthBar()
    {
        //float dmg = (float)playerShipController.GetDamageTaken() / SaveData.instance.playerShipHealth;
        StartCoroutine(DelayExecuationOfHealthBar());
        //Debug.Log("health bar minus " + dmg);
    }

    IEnumerator DelayExecuationOfHealthBar()
    {
        yield return new WaitForSeconds(1f);
        playerHealthBar.fillAmount = SaveData.instance.playerShipHealth / SaveData.instance.playerShipTotalHealth;

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
        PlayerShipController.OnPlayerTakeDamage += UpdatePlayerHealthBar;
        PlayerShipController.OnPlayerRespawn += ActivateGameOverSequence;
        ShipGameManager.onStageClear += UpdateStageText;
        ShipGameManager.onStageClear += UpdateStageTextPosition;

    }

    private void OnDisable()
    {
        PlayerShipController.OnPlayerTakeDamage -= UpdatePlayerHealthBar;
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