using Unity.Netcode;

public class PlayerInventory : NetworkBehaviour
{
    public NetworkList<InventorySlot> inventory = new NetworkList<InventorySlot>();

    public void AddItem(int itemId, int amount)
    {
        if (!IsServer) return;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemId == itemId)
            {
                InventorySlot slot = inventory[i];
                slot.count += amount;
                inventory[i] = slot;
                return;
            }
        }
        inventory.Add(new InventorySlot(itemId, amount));
    }
}
