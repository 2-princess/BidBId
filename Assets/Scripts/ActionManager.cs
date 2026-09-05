using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ActionManager : NetworkBehaviour
{
    public static ActionManager Instance;
    private CardData selectedCard; // 카드정보
    public NetworkVariable<CardGrade> currentGrade = new NetworkVariable<CardGrade>(); // 공개정보
    private ulong informationClientId; // 정보아는 플레이어
    [SerializeField] private ActionUI auctionUI; // 스킬정보 혼자알게
    public NetworkVariable<int> currentPrice = new NetworkVariable<int>(0); // 입찰가
    public NetworkVariable<ulong> highestBidder = new NetworkVariable<ulong>(ulong.MaxValue); // 최고입찰자

    public NetworkVariable<AuctionState> auctionState = new NetworkVariable<AuctionState>(AuctionState.Ready);
    public enum AuctionState
    {
        Ready,
        Information,
        Bidding,
        Result
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!IsServer) return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            SelectRandomCard();
        }

    }

    public void SelectRandomCard()
    {
        if (!IsServer) return;

        selectedCard = CardDatabase.Instance.GetRandomCard();
        currentGrade.Value = selectedCard.grade;
        currentPrice.Value = 10000;
        Debug.Log("경매 카드 : " + selectedCard.cardName);
        SelectInformationPlayer();
        auctionState.Value = AuctionState.Information;
        ShowSecretSkillRpc(selectedCard.skillDescription, RpcTarget.Single(informationClientId, RpcTargetUse.Temp));
        StartCoroutine(InformationTimer());
    }

    private void SelectInformationPlayer()
    {
        if (!IsServer) return;
        var clients = NetworkManager.Singleton.ConnectedClientsIds;
        if (clients.Count == 0) return;
        int randomIndex = Random.Range(0, clients.Count);
        informationClientId = clients[randomIndex];

        Debug.Log("정보를 아는 플레이어 : " + informationClientId);
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void ShowSecretSkillRpc(string skillDescription, RpcParams rpcParams = default)
    {
        if (auctionUI == null)
        {
            Debug.Log("auctionUI가 null");
            return;
        }
        auctionUI.ShowSecretSkill(skillDescription);
    }

    private void StartBidding()
    {
        if (!IsServer) return;

        auctionState.Value = AuctionState.Bidding;

        Debug.Log("경매 입찰 시작");
    }

    private IEnumerator InformationTimer()
    {
        yield return new WaitForSeconds(5f);

        StartBidding();
    }

    [Rpc(SendTo.Server)]
    public void BidRpc(int amount, RpcParams rpcParams = default)
    {
        if (auctionState.Value != AuctionState.Bidding) return;

        if (amount <= currentPrice.Value) return;

        ulong bidderId = rpcParams.Receive.SenderClientId;
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(bidderId, out NetworkClient client)) return;

        PlayerStatus status = client.PlayerObject.GetComponent<PlayerStatus>();
        if (status.gold.Value < amount)
        {
            Debug.Log("골드 부족");
            return;
        }
        currentPrice.Value = amount;
        highestBidder.Value = bidderId;

        Debug.Log("현재 입찰가 : " + currentPrice.Value);
        Debug.Log("최고 입찰자 : " + highestBidder.Value);
    }
}
