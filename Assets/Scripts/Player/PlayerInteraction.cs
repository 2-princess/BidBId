using Unity.Netcode;
using UnityEngine;

public class PlayerInteraction : NetworkBehaviour
{
    public PlayerAnimationController aniCon;
    public PlayerMoveController playerMoveCon;
    bool isMining = false;
    int pressCount = 0;
    float checkTimer = 0f;

    void Update()
    {
        if (!IsOwner) return;
        if (isMining)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.EscPanelToggle(false);
                playerMoveCon.isMove = true;
                isMining = false;
            }
            checkTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider col in cols)
            {
                if (col.CompareTag("Ore"))
                {
                    GameManager.Instance.EscPanelToggle(true);
                    pressCount++;
                    Debug.Log("E키 누름");
                    isMining = true;
                    playerMoveCon.isMove = false;
                    GetOreRpc();
                    aniCon.SetAni(PlayerAnimationController.PlayerState.Mining);
                    break;
                }
                if (col.CompareTag("Store"))
                {
                    GameManager.Instance.StoreToggle();
                }
            }
        }
        if (checkTimer >= 2f)
        {
            Debug.Log("2초 동안 누른 횟수 : " + pressCount);
            if (pressCount > 14) MiningSpeed(7f);
            else if (pressCount > 8) MiningSpeed(5f);
            else MiningSpeed(1.5f);

            pressCount = 0;
            checkTimer = 0f;
        }
    }

    void MiningSpeed(float speed)
    {
        aniCon.SetMiningSpeed(speed);
    }

    [Rpc(SendTo.Server)]
    void GetOreRpc()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Ore"))
            {
                OreNode ore = col.GetComponent<OreNode>();
                PlayerInventory inventory = GetComponent<PlayerInventory>();
                ore.HpMinus(inventory);
                break;
            }
        }
    }
}
