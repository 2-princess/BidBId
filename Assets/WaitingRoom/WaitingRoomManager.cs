using TMPro;
using Unity.Netcode;
using UnityEngine;

public class WaitingRoomManager : NetworkBehaviour
{
    public static WaitingRoomManager Instance;
    public NetworkVariable<int> playerCount = new NetworkVariable<int>();
    public NetworkList<ulong> readyPlayers = new NetworkList<ulong>();
    public TMP_Text roomCode;
    [SerializeField] private TMP_Text playerCountText;

    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        playerCount.OnValueChanged += OnPlayerCountChanged;
        OnPlayerCountChanged(0, playerCount.Value);
        roomCode.text = "CODE : " + (SessionManager.Instance.roomCode).ToString();
        if (!IsServer) return;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        playerCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    public override void OnNetworkDespawn()
    {
        playerCount.OnValueChanged -= OnPlayerCountChanged;
    }

    private void OnClientConnected(ulong clientId)
    {
        playerCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        playerCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    private void OnPlayerCountChanged(int oldValue, int newValue)
    {
        playerCountText.text = newValue + " / 8";
    }

}
