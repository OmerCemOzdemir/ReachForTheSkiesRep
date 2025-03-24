using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//[ExecuteInEditMode()] makes it so that you can run the code shile the game is not running
//[ExecuteInEditMode()]
public class ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;



    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        contentField.text = content;
        UpdateLayoutElementSize();
    }

    private void UpdateLayoutElementSize()
    {
        int headerLenght = headerField.text.Length;
        int contentLenght = contentField.text.Length;
        // layoutElement.enabled = (headerLenght > characterWrapLimit || contentLenght > characterWrapLimit) ? false : true;
        layoutElement.enabled = Math.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;

    }

    private void PositionToolTip()
    {
        Vector2 position = Input.mousePosition;
        //true = toolTip is on left, false = toolTip is on Right
        bool toolTipPos = (GetComponent<RectTransform>().position.x < Screen.width / 2) ? true : false;
        if (toolTipPos)
        {
            GetComponent<RectTransform>().pivot = Vector3.zero;
        }
        else
        {
            GetComponent<RectTransform>().pivot = Vector3.one;

        }

        transform.position = position;
    }

    private void Update()
    {
        PositionToolTip();
    }

    public void DebugCertainElement()
    {
        Debug.Log("Screen Width /2: " + Screen.width / 2);
        Debug.Log("Screen Width /2: " + Screen.width / 2 + "rectTransform.x : " + GetComponent<RectTransform>().position.x);
        bool toolTipPos = (GetComponent<RectTransform>().position.x < Screen.width / 2) ? true : false;
        Debug.Log("Is TooTip On the Left: " + toolTipPos);
    }

}
/*
 
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DebugCertainElement();
        }


        Vector2 anchoredPosition = Input.mousePosition / gameObject.transform.root.GetComponent<RectTransform>().localScale.x;

        if (anchoredPosition.x + GetComponent<RectTransform>().rect.width > gameObject.transform.root.GetComponent<RectTransform>().rect.width)
        {
            anchoredPosition.x = gameObject.transform.root.GetComponent<RectTransform>().rect.width - GetComponent<RectTransform>().rect.width;
        }
        if (anchoredPosition.y + GetComponent<RectTransform>().rect.height > gameObject.transform.root.GetComponent<RectTransform>().rect.height)
        {
            anchoredPosition.y = gameObject.transform.root.GetComponent<RectTransform>().rect.height - GetComponent<RectTransform>().rect.height;
        }

        GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
 
 */