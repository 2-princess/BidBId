using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameSpawnManager : NetworkBehaviour
{
    public Transform[] spawnPoints;

    IEnumerator Start()
    {
        // 내 PlayerObject가 준비될 때까지 기다림   
        yield return new WaitUntil(() => NetworkManager.Singleton.LocalClient.PlayerObject != null);

        NetworkObject player = NetworkManager.Singleton.LocalClient.PlayerObject;

        ulong clientId = NetworkManager.Singleton.LocalClientId;

        int spawnIndex = (int)(clientId % (ulong)spawnPoints.Length);

        player.transform.position = spawnPoints[spawnIndex].position;
    }
}
