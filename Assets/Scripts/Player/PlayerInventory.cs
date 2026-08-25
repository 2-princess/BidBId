using UnityEngine;
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

    [Rpc(SendTo.Server)]
    public void SellItemRpc(int id, int amount)
    {
        if (amount <= 0) return;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemId != (int)id) continue;
            // 실제 보유량보다 많이 팔려고 하면 거부
            if (inventory[i].count < amount) return;

            InventorySlot slot = inventory[i];
            slot.count -= amount;
            Debug.Log("슬롯카운트감소");

            // 전부 팔았으면 슬롯 삭제
            if (slot.count <= 0)
            {
                inventory.RemoveAt(i);
            }
            else
            {
                inventory[i] = slot;
            }
            int price = 0;

            switch ((ItemId)id)
            {
                case ItemId.Iron:
                    price = 2000;
                    break;

                case ItemId.Copper:
                    price = 5000;
                    break;

                case ItemId.Gold:
                    price = 10000;
                    break;
            }
            PlayerStatus status = GetComponent<PlayerStatus>();
            status.AddGold(price * amount);
            return;
        }
        Debug.Log("판매하려는 아이템을 인벤토리에서 못 찾음");
    }
}
