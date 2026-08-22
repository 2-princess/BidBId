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
        if (isMining) checkTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider col in cols)
            {
                if (col.CompareTag("Ore"))
                {
                    pressCount++;
                    isMining = true;
                    playerMoveCon.isMove = false;
                    NetworkObject ore = col.GetComponent<NetworkObject>();
                    GetOreRpc(ore);
                    aniCon.SetAni(PlayerAnimationController.PlayerState.Mining);
                    break;
                }
            }
        }
        if (checkTimer >= 2f)
        {
            Debug.Log("2초 동안 누른 횟수 : " + pressCount);
            if (pressCount > 9) MiningSpeed(7f);
            else if (pressCount > 7) MiningSpeed(5f);
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
    void GetOreRpc(NetworkObjectReference col)
    {
        if (col.TryGet(out NetworkObject netObj))
        {
            OreNode ore = netObj.GetComponent<OreNode>();
            ore.hpMinus();
        }
    }
}
