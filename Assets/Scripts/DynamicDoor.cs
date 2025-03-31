using UnityEngine;

public class DynamicDoor : MonoBehaviour
{
    [SerializeField] private LerpObject lerpObject;

    private void Start()
    {
        //lerpObject = GetComponent<LerpObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCircle"))
        {
            lerpObject.LerpObjectToPoint2();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCircle"))
        {
            lerpObject.LerpObjectBack();
        }
    }

}
