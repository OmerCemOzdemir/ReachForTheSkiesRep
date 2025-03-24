using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [TextArea]
    [SerializeField] private string content;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipSystem.ShowToolTip(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.HideToolTip();

    }
}
