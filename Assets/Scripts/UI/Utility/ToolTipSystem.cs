using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;

    public ToolTip toolTip;

    private void Awake()
    {
        instance = this;
    }

    public static void ShowToolTip(string content, string header = "")
    {
        instance.toolTip.SetText(content, header);
        instance.toolTip.gameObject.SetActive(true);
    }
    public static void HideToolTip()
    {
        instance.toolTip.gameObject.SetActive(false);

    }
}
