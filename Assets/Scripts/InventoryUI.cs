using Unity.Netcode;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Sprite[] itemSprites;
    private PlayerInventory playerInventory;

    private void OnInventoryChanged(NetworkListEvent<InventorySlot> changeEvent)
    {
        RefreshInventory();
    }

    public void OpenInventory()
    {
        playerInventory = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInventory>();

        playerInventory.inventory.OnListChanged -= OnInventoryChanged;
        playerInventory.inventory.OnListChanged += OnInventoryChanged;

        int slotCount = Mathf.Max(16, playerInventory.inventory.Count);
        if (content.childCount < slotCount)
        {
            for (int i = 0; i < slotCount - 16; i++)
            {
                Instantiate(itemSlotPrefab, content);
            }
        }
        RefreshInventory();
    }

    private void RefreshInventory()
    {
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
