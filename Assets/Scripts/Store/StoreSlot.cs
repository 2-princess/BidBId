using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemNeedText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private TMP_Text sellCountText;
    private int sellCount = 1;
    private int sellMax = 1;
    private int itemId;

    public void SetStore(Sprite img, int itemNeed, int itemPrice, int id)
    {
        itemImage.sprite = img;
        itemImage.enabled = true;
        itemNameText.text = ((ItemId)id).ToString();
        itemNeedText.text = "Need : " + itemNeed.ToString();
        itemPriceText.text = itemPrice.ToString();
        sellMax = itemNeed;
        itemId = id;
        sellCount = 1;
        sellCountText.text = sellCount.ToString();
    }

    public void BtnPlus()
    {
        if (sellCount < sellMax)
            sellCount++;
        sellCountText.text = sellCount.ToString();
    }
    public void BtnMinus()
    {
        if (sellCount > 1)
            sellCount--;
        sellCountText.text = sellCount.ToString();
    }

    public void SellBtn()
    {
        PlayerInventory inventory = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInventory>();
        inventory.SellItemRpc(itemId, sellCount);
    }
}
