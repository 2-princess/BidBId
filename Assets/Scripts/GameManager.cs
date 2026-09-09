using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CardInventoryUI cardInventoryUI;
    public InventoryUI inventoryUI;

    public TargetSelectUI targetSelectUI;
    public GameObject storePanel;
    public GameObject escPanel;
    public StoreUI storeUI;
    public InterferenceUI interferenceUI;

    void Awake()
    {
        Instance = this;
    }

    public void PlayersPanelOpen(int cardId)
    {
        targetSelectUI.gameObject.SetActive(!targetSelectUI.gameObject.activeInHierarchy);
        if (targetSelectUI.gameObject.activeInHierarchy)
        {
            targetSelectUI.Open(cardId);
        }
    }

    public void CardInventoryToggle()
    {
        cardInventoryUI.gameObject.SetActive(!cardInventoryUI.gameObject.activeInHierarchy);
        if (cardInventoryUI.gameObject.activeInHierarchy)
        {
            cardInventoryUI.OpenInventory();
        }
    }

    public void InventoryToggle()
    {
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeInHierarchy);
        if (inventoryUI.gameObject.activeInHierarchy)
        {
            inventoryUI.OpenInventory();
        }
    }

    public void StoreToggle()
    {
        storePanel.SetActive(!storePanel.activeInHierarchy);

        if (storePanel.activeInHierarchy)
        {
            storeUI.OpenStore();
        }
    }

    public void EscPanelToggle(bool state)
    {
        escPanel.SetActive(state);
    }

}
