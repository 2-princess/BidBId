using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    public NetworkVariable<int> gold = new NetworkVariable<int>();

    public void AddGold(int amount)
    {
        if (!IsServer) return;
        if (amount <= 0) return;
        gold.Value += amount;
    }

    public bool RemoveGold(int amount)
    {
        if (!IsServer) return false;
        if (amount <= 0) return false;
        if (gold.Value < amount) return false;

        gold.Value -= amount;
        return true;
    }
}
