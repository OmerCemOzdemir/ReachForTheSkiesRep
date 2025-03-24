using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject infoText;

    public static InfoPanel instance;

    private void Awake()
    {
        instance = this;
    }


    public void TriggerInfoText(string content, Color giveColor)
    {
        GameObject newInfoText = Instantiate(infoText);
        newInfoText.GetComponentInChildren<TextMeshProUGUI>().color = giveColor;
        newInfoText.GetComponentInChildren<TextMeshProUGUI>().text = content;
        newInfoText.transform.SetParent(transform, false);
        Debug.Log("It Triggered InfoText ");
        StartCoroutine(FadeText(4, newInfoText));
    }



    IEnumerator FadeText(int timer, GameObject newInfoText)
    {

        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1f);
            newInfoText.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
            newInfoText.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(0, 0, 0, 0.1f);
        }

        yield return new WaitForSeconds(2f);

        Destroy(newInfoText);
        newInfoText = null;

    }
}
