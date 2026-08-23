using TMPro;
using Unity.Netcode;
using UnityEngine;
using UniversityOfGames.ProgressBarToolkit;

public class OreNode : NetworkBehaviour
{

    public NetworkVariable<float> hp = new NetworkVariable<float>(40);
    public SegmentedProgressBar progressBar;
    public TMP_Text text;

    public override void OnNetworkSpawn()
    {
        hp.OnValueChanged += HpChanged;
        progressBar.FillAmount = 1;
        text.text = "HP : " + hp.Value;
    }

    private void HpChanged(float oldValue, float newValue)
    {
        Debug.Log("광석 HP 변경 : " + oldValue + " → " + newValue);
        progressBar.FillAmount = newValue / 40;
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
