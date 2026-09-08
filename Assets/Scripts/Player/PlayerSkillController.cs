using Unity.Netcode;
using UnityEngine;

public class PlayerSkillController : NetworkBehaviour
{
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            UseCard(1);
        }
    }

    public void UseCard(int cardId)
    {
        if (!IsOwner) return;
        CardData card = CardDatabase.Instance.GetCard(cardId);

        if (card == null)
        {
            Debug.Log("카드 데이터 없음");
            return;
        }

        if (card.needTarget)
        {
            // 타겟 선택 UI 열기
            GameManager.Instance.PlayersPanelOpen(cardId);
        }
        else
        {
            // 타겟이 필요 없으므로 바로 사용
            UseCardRpc(cardId, ulong.MaxValue);
        }
    }

    [Rpc(SendTo.Server)]
    public void UseCardRpc(int cardId, ulong targetClientId)
    {
        PlayerStatus userStatus = GetComponent<PlayerStatus>();
        if (userStatus == null)
        {
            Debug.Log("사용자의 PlayerStatus 없음");
            return;
        }
        if (!userStatus.cards.Contains(cardId))
        {
            Debug.Log("보유하지 않은 카드");
            return;
        }

        CardData card = CardDatabase.Instance.GetCard(cardId);
        if (card == null)
        {
            Debug.Log("카드 데이터 없음");
            return;
        }
        if (card.skill == null)
        {
            Debug.Log("카드에 스킬이 연결되어 있지 않음");
            return;
        }

        PlayerStatus targetStatus = null;
        if (card.needTarget)
        {
            if (targetClientId == OwnerClientId)
            {
                Debug.Log("자기 자신에게는 사용할 수 없음");
                return;
            }
            if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out NetworkClient targetClient))
            {
                Debug.Log("대상 플레이어를 찾을 수 없음");
                return;
            }
            targetStatus = targetClient.PlayerObject.GetComponent<PlayerStatus>();
            if (targetStatus == null)
            {
                Debug.Log("PlayerStatus 없음");
                return;
            }
        }
        card.skill.Use(userStatus, targetStatus);

    }
}