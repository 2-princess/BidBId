using Unity.Netcode;
using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    RoundEnd
}

public class GameStateManager : NetworkBehaviour
{
    public static GameStateManager Instance;

    public NetworkVariable<int> round = new(1);
    public NetworkVariable<GameState> gameState = new(GameState.Ready);

    private void Awake()
    {
        Instance = this;
    }

    public void StartRound()
    {
        if (!IsServer) return;

        gameState.Value = GameState.Playing;

        Debug.Log(round.Value + " 라운드 시작");
    }

    public void EndRound()
    {
        if (!IsServer) return;

        gameState.Value = GameState.RoundEnd;

        Debug.Log(round.Value + " 라운드 종료");

        round.Value++;
    }
}