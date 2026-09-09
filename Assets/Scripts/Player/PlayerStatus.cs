using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    public NetworkVariable<int> gold = new NetworkVariable<int>();
    public NetworkList<int> cards = new NetworkList<int>();
    public NetworkVariable<FixedString64Bytes> nickname = new NetworkVariable<FixedString64Bytes>();
    public int minerLuckChance = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) AddCard(30);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        SetNicknameRpc(SessionManager.Instance.MyNickname);
        Debug.Log("접속한 닉네임 : " + nickname.Value);
    }

    [Rpc(SendTo.Server)]
    private void SetNicknameRpc(string newNickname)
    {
        nickname.Value = newNickname;
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
