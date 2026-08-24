using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text countText;

    public void SetImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }

    public void SetCount(int count)
    {
        countText.text = count.ToString();
    }
}
