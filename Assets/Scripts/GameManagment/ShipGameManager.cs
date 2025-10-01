using System;
using UnityEngine;

public class ShipGameManager : MonoBehaviour
{
    [HideInInspector] public float timer = 0f;


    [Header("Set Game Stages: ")]
    [SerializeField] private float[] stageInterval;
    [SerializeField] private float debugStageInterval;

    private int gameLevel;

    public static event Action<int> onStageClear;
    public static event Action onBossEncounter;
    bool onetime0 = true;
    bool onetime1 = true;
    bool onetime2 = true;

    void Awake()
    {
        if (SaveData.instance.newGame)
        {
            SaveData.instance.gameShipLevel = 1;
            SaveData.instance.gameShipStageTimer = 0;

        }
        else
        {
            gameLevel = SaveData.instance.gameShipLevel;
            timer = SaveData.instance.gameShipStageTimer;

        }

        Physics.IgnoreLayerCollision(7, 6);
        //Physics.IgnoreLayerCollision(6, 6);
        //--------------------------------------------------

    }

    private void Start()
    {
        if (debugStageInterval != 0)
        {
            timer = debugStageInterval;
        }
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (timer > stageInterval[0])
        {
            if (onetime0)
            {
                onStageClear?.Invoke(2);
                onetime0 = false;
                SaveData.instance.gameShipStageTimer = stageInterval[0];

            }
        }
        if (timer > stageInterval[1])
        {
            if (onetime1)
            {
                onStageClear?.Invoke(3);
                //UnityEngine.Debug.Log("In Second Stage ");
                Debug.Log("Stage 2 is here");
                onetime1 = false;
                SaveData.instance.gameShipStageTimer = stageInterval[1];

            }
        }
        if (timer > stageInterval[2])
        {
            if (onetime2)
            {
                //UnityEngine.Debug.Log("In Second Stage ");
                onBossEncounter?.Invoke();
                onetime2 = false;
                SaveData.instance.gameShipStageTimer = stageInterval[2];

            }
        }

    }

}
