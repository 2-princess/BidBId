using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject inventoryPanel;
    public InventoryUI inventoryUI;
    public GameObject storePanel;
    public GameObject escPanel;
    public StoreUI storeUI;

    void Awake()
    {
        Instance = this;
    }

    public void InventoryToggle()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        if (inventoryPanel.activeInHierarchy)
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
