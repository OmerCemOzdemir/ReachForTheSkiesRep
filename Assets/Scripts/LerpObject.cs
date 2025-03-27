using System.Collections;
using UnityEngine;

public class LerpObject : MonoBehaviour
{
    [SerializeField] public float lerpSpeed;
    private float timePassed;
    private float timeComplete;
    private Vector3 endVector;
    private Vector3 startVector;
    private Vector3 endVector2;

    private Vector3 storeStartVector;
    private Vector3 storeEndVector;
    private bool active = false;

    [HideInInspector] public bool startLerp = false;
    [HideInInspector] public bool stopLerp = false;

    private void Start()
    {
        startVector = transform.position;
        endVector = transform.GetChild(0).position;
        storeStartVector = startVector;
        storeEndVector = endVector;
        endVector2 = transform.GetChild(1).position;
    }


    public void LerpObjectToPoint()
    {
        StartCoroutine(LerpObjectCoroutine(startVector, endVector, lerpSpeed));

    }

    public void LerpObjectToPoint2()
    {
        StartCoroutine(LerpObjectCoroutine(endVector, endVector2, lerpSpeed));

    }

    public void LerpObjectBack()
    {
        StartCoroutine(LerpObjectCoroutine(endVector, startVector, lerpSpeed));

    }

    IEnumerator LerpObjectCoroutine(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;

    }

}
