using Unity.Netcode;
using UnityEngine;

public class PlayerCardController : NetworkBehaviour
{
    [Rpc(SendTo.Server)]
    public void UseCardRpc(int cardId, ulong targetClientId)
    {
        Debug.Log("카드 사용 요청 : " + cardId);
    }
}
