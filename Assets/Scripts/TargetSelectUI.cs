using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectUI : NetworkBehaviour
{
    public Transform trans;
    public GameObject namePrefeb;
    public int selectedCardId;

    public void Open(int cardId)
    {
        selectedCardId = cardId;
        SelectPlayers();
    }

    public void Close()
    {
        GameManager.Instance.targetSelectUI.gameObject.SetActive(false);
    }

    public void SelectPlayers()
    {
        foreach (Transform child in trans)
        {
            Destroy(child.gameObject);
        }
        foreach (NetworkObject netObj in NetworkManager.Singleton.SpawnManager.SpawnedObjectsList)
        {
            if (!netObj.IsPlayerObject) continue;
            if (netObj.OwnerClientId == NetworkManager.Singleton.LocalClientId) continue;

            PlayerStatus status = netObj.GetComponent<PlayerStatus>();
            if (status == null) continue;

            GameObject buttonObj = Instantiate(namePrefeb, trans);

            TargetPlayerName targetPlayerName = buttonObj.GetComponent<TargetPlayerName>();

            if (targetPlayerName != null)
            {
                targetPlayerName.nameText.text = status.nickname.Value.ToString();
            }

            Button button = buttonObj.GetComponent<Button>();

            ulong targetClientId = netObj.OwnerClientId;

            button.onClick.AddListener(() =>
            {
                PlayerSkillController skillController = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerSkillController>();
                skillController.UseCardRpc(selectedCardId, targetClientId);
                Close();
            });
        }
    }
}
