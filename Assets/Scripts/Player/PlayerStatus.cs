using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    public NetworkVariable<int> gold = new NetworkVariable<int>();
    public NetworkList<int> cards = new NetworkList<int>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) AddCard(2);
    }

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

    public void AddCard(int cardId)
    {
        if (!IsServer) return;
        cards.Add(cardId);
        Debug.Log("카드 획득 : " + cardId);
    }
}
