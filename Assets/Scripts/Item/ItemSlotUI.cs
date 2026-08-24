using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private TMP_Text itemNameText;

    public void SetItem(Sprite sprite, string itemName, int count)
    {
        itemImage.sprite = sprite;
        itemImage.enabled = true;

        itemNameText.text = itemName;
        countText.text = count.ToString();
    }
    public void Clear()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemNameText.text = "";
        countText.text = "";
    }
}
