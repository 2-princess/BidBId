using Unity.Netcode;
using UnityEngine;

public class CardInventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject cardInventoryPanel;
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private Transform content;

    private PlayerStatus playerStatus;

    public void OpenInventory()
    {
        cardInventoryPanel.gameObject.SetActive(!cardInventoryPanel.gameObject.activeInHierarchy);
        playerStatus = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerStatus>();
        RefreshInventory();
    }

    void RefreshInventory()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (int cardId in playerStatus.cards)
        {
            CardData cardData = CardDatabase.Instance.GetCard(cardId);

            if (cardData == null)
            {
                Debug.LogWarning("카드 데이터를 찾을 수 없음 : " + cardId);
                continue;
            }

            GameObject slot = Instantiate(cardSlotPrefab, content);

            CardSlotUI slotUI = slot.GetComponent<CardSlotUI>();
            slotUI.SetCard(cardData);
        }
    }
}