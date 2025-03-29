using System.Collections;
using UnityEngine;

public class LerpObject : MonoBehaviour
{
    [SerializeField] public float lerpSpeed;
    private Vector3 endVector;
    private Vector3 startVector;
    private Vector3 endVector2;

    private void Start()
    {
        startVector = transform.position;
        endVector = transform.GetChild(0).position;
        endVector2 = transform.GetChild(1).position;
    }


    public void LerpObjectToPoint()
    {
        StartCoroutine(LerpObjectCoroutine(startVector, endVector, lerpSpeed));
        //Debug.Log("LerpStarted");
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
