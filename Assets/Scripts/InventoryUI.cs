using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Sprite[] itemSprites;
    [SerializeField] private TMP_Text goldText;
    private PlayerInventory playerInventory;
    private PlayerStatus playerStatus;

    private void OnInventoryChanged(NetworkListEvent<InventorySlot> changeEvent)
    {
        RefreshInventory();
    }
    private void OnGoldChanged(int oldValue, int newValue)
    {
        goldText.text = "GOLD : " + newValue.ToString();
    }

    public void OpenInventory()
    {
        GameObject player = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;

        playerInventory = player.GetComponent<PlayerInventory>();
        playerStatus = player.GetComponent<PlayerStatus>();

        playerInventory.inventory.OnListChanged -= OnInventoryChanged;
        playerInventory.inventory.OnListChanged += OnInventoryChanged;

        playerStatus.gold.OnValueChanged -= OnGoldChanged;
        playerStatus.gold.OnValueChanged += OnGoldChanged;

        int slotCount = Mathf.Max(12, playerInventory.inventory.Count);
        if (content.childCount < slotCount)
        {
            for (int i = 0; i < slotCount - 12; i++)
            {
                Instantiate(itemSlotPrefab, content);
            }
        }
        RefreshInventory();
    }

    private void RefreshInventory()
    {
        goldText.text = "GOLD : " + playerStatus.gold.Value.ToString();
        for (int i = 0; i < content.childCount; i++)
        {
            ItemSlotUI slotUI = content.GetChild(i).GetComponent<ItemSlotUI>();

            if (i < playerInventory.inventory.Count)
            {
                InventorySlot item = playerInventory.inventory[i];

                Sprite sprite = itemSprites[item.itemId - 1];
                string itemName = ((ItemId)item.itemId).ToString();
                slotUI.SetItem(sprite, itemName, item.count);
            }
            else
            {
                slotUI.Clear();
            }
        }
    }
}
