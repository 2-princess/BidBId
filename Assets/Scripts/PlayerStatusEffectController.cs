using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatusEffectController : NetworkBehaviour
{
    public float MoveSpeedMultiplier { get; private set; } = 1f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        StartCoroutine(WaitGameStateManager());
    }
    private IEnumerator WaitGameStateManager()
    {
        yield return new WaitUntil(() => GameStateManager.Instance != null);

        GameStateManager.Instance.gameState.OnValueChanged += OnGameStateChanged;
    }


    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.gameState.OnValueChanged
                -= OnGameStateChanged;
        }
    }

    public void StartOvertime(float multiplier)
    {
        if (!IsServer) return;
        StartOvertimeRpc(multiplier);
    }

    [Rpc(SendTo.Owner)]
    private void StartOvertimeRpc(float multiplier)
    {
        MoveSpeedMultiplier = multiplier;
    }

    private void OnGameStateChanged(GameState oldState, GameState newState)
    {
        if (newState == GameState.RoundEnd)
        {
            MoveSpeedMultiplier = 1f;

            Debug.Log("라운드 종료 - 상태효과 초기화");
        }
    }
}