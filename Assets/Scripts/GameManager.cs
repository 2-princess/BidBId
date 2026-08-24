using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public InventoryUI inventoryUI;

    public void InventoryToggle()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        if (inventoryPanel.activeInHierarchy)
        {
            inventoryUI.OpenInventory();
        }
    }
}
