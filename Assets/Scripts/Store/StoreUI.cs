using Unity.Netcode;
using UnityEngine;

public class StoreUI : NetworkBehaviour
{
    [SerializeField] private GameObject storeSlotPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Sprite[] itemSprites;

    private PlayerInventory playerInventory;

    public void OpenStore()
    {
        playerInventory = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInventory>();
        playerInventory.inventory.OnListChanged -= OnInventoryChanged;
        playerInventory.inventory.OnListChanged += OnInventoryChanged;
        RefreshStore();
    }

    private void OnInventoryChanged(NetworkListEvent<InventorySlot> changeEvent)
    {
        RefreshStore();
    }

    public void RefreshStore()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        playerInventory = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInventory>();
        foreach (InventorySlot item in playerInventory.inventory)
        {
            GameObject slotObject = Instantiate(storeSlotPrefab, content);

            StoreSlot storeSlot = slotObject.GetComponent<StoreSlot>();
            storeSlot.SetStore(itemSprites[item.itemId - 1], item.count, 20000, item.itemId);
        }
    }


}