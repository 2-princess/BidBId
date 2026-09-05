using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameSpawnManager : NetworkBehaviour
{
    public Transform[] spawnPoints;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.LocalClient.PlayerObject != null);

        RequestSpawnRpc();
    }

    [Rpc(SendTo.Server)]
    void RequestSpawnRpc(RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(
            clientId,
            out NetworkClient client))
            return;

        NetworkObject player = client.PlayerObject;

        int spawnIndex = (int)(clientId % (ulong)spawnPoints.Length);

        Vector3 spawnPosition =
            spawnPoints[spawnIndex].position + Vector3.up;

        PlayerMoveController move = player.GetComponent<PlayerMoveController>();

        move.SpawnTeleportRpc(spawnPosition);

        Debug.Log($"플레이어 {clientId} 스폰 위치 : {spawnPosition}");
    }
}