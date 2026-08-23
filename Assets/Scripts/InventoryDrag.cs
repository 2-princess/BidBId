using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private Canvas canvas;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 size = inventoryPanel.sizeDelta;
        size.y -= eventData.delta.y;
        inventoryPanel.sizeDelta = size;
    }
}
