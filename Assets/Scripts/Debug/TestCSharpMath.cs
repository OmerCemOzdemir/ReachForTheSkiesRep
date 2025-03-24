using TMPro;
using UnityEngine;

public class TestCSharpMath : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;

    float num1 = 60;
    float num2 = 100;

    void Start()
    {
        answerText.text = "" + (num1 / num2);


    }

}
