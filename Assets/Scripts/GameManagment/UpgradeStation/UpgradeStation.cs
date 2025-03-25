using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private Transform[] pointTransforms;
    [SerializeField] private float timeToMove;
    [SerializeField] private float timeToStay;
    [SerializeField] private float timeToRepeat;
    [SerializeField] private float timeToStart;

    [SerializeField] private TextMeshProUGUI textForCountdown;
    [SerializeField] private Material material;
    float timeElapsed;


    private void Start()
    {
        //InvokeRepeating(nameof(MoveUpgradeStationRandom), timeToStart, timeToRepeat);
        InvokeRepeating(nameof(MoveUpgradeStationRandom), timeToStart, timeToRepeat);
        //MoveUpgradeStationRandom();
    }

    private void MoveUpgradeStationRandom()
    {
        Debug.Log("Upgrade Station is Started");
        StartCoroutine(LerpFunction(0, 1, timeToMove));
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void WaitUpgradeStation()
    {
        
        textForCountdown.gameObject.SetActive(true);
        GetComponentInChildren<ParticleSystem>().Stop();
        StartCoroutine(UpgradeStationDelay(timeToStay));
        Invoke("EndUpgradeStation", timeToStay);

    }


    private void EndUpgradeStation()
    {
        textForCountdown.gameObject.SetActive(false);
        material.SetColor("_OutlineColor", new Color(0, 0, 0));
        material.SetColor("_OutlineColor2", new Color(0, 0, 0));
        GetComponentInChildren<ParticleSystem>().Play();

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(LerpFinal(pointTransforms[1].position, pointTransforms[2].position, timeToMove));

    }


    IEnumerator UpgradeStationDelay(float wait)
    {

        while (wait > 0)
        {
            textForCountdown.text = "" + wait;
            material.SetColor("_OutlineColor", new Color(0.1569983f, 1, 0));
            material.SetColor("_OutlineColor2", new Color(0, 0.7353768f, 1));
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            GetComponent<BoxCollider2D>().enabled = true;
            //Debug.Log("Time remaining: " + wait);
            yield return new WaitForSeconds(1f);


            wait--;
        }

    }


    IEnumerator LerpFunction(int pointIndexStart, int pointIndexEnd, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(pointTransforms[pointIndexStart].position, pointTransforms[pointIndexEnd].position, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = pointTransforms[pointIndexEnd].position;
        WaitUpgradeStation();

    }


    IEnumerator LerpFinal(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        //transform.position = target;
        FinalizeUpgradeStationSequence();

    }

    private void FinalizeUpgradeStationSequence()
    {
        transform.position = pointTransforms[0].position;
        StopAllCoroutines();
    }


    private void StopUpgradeShipSquence()
    {
        CancelInvoke(nameof(MoveUpgradeStationRandom));

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveManager.saveManager_Instance.SaveGame();
            SceneManager.LoadScene(2);

        }
    }

    private void OnEnable()
    {
        ShipGameManager.onBossEncounter += StopUpgradeShipSquence;
    }

    private void OnDisable()
    {
        ShipGameManager.onBossEncounter -= StopUpgradeShipSquence;

    }
}
/*
 
    IEnumerator LerpFunction(float wait, int pointIndexA, int pointIndexB)
    {
        while (wait > 0)
        {
            timeElapsed += Time.deltaTime;
            float persentageComplete = timeElapsed / timeToMove;
            yield return new WaitForSeconds(1f);
            transform.position = Vector3.Lerp(pointTransforms[pointIndexA].position, pointTransforms[pointIndexB].position, persentageComplete);

            wait--;
        }

    }

 
  
        //StartCoroutine(UpgradeStationDelay(timeToStay));

 
 */