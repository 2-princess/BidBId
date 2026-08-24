using Unity.Netcode;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform content;
    public void OpenInventory()
    {
        PlayerInventory playerInventory = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerInventory>();

        int slotCount = Mathf.Max(16, playerInventory.inventory.Count);
        if (slotCount > 16)
        {
            for (int i = 0; i < slotCount - 16; i++)
            {
                Instantiate(itemSlotPrefab, content);
            }
        }
    }
}
