using TMPro;
using Unity.Netcode;
using UnityEngine;

public class OreNode : NetworkBehaviour
{

    public NetworkVariable<int> hp = new NetworkVariable<int>(40);
    public TMP_Text text;

    public override void OnNetworkSpawn()
    {
        hp.OnValueChanged += HpChanged;
        text.text = "HP : " + hp.Value;
    }

    private void HpChanged(int oldValue, int newValue)
    {
        Debug.Log("광석 HP 변경 : " + oldValue + " → " + newValue);
        text.text = "HP : " + newValue;
    }
    public override void OnNetworkDespawn()
    {
        hp.OnValueChanged -= HpChanged;
    }

    public void HpMinus(PlayerInventory playerInventory)
    {
        if (!IsServer) return;
        hp.Value--;
        if (hp.Value <= 0)
        {
            Reward(playerInventory);
            hp.Value = 40;
        }
    }
    void Reward(PlayerInventory inventory)
    {
        ItemId rewardItem = GetRandomOre();
        inventory.AddItem((int)rewardItem, 1);
        Debug.Log("획득한 아이템 : " + rewardItem);
    }

    ItemId GetRandomOre()
    {
        int random = Random.Range(0, 100);

        if (random < 50)
            return ItemId.Iron;

        if (random < 80)
            return ItemId.Copper;

        if (random < 95)
            return ItemId.Gold;

        return ItemId.Diamond;
    }
}
