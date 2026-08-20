using Unity.Netcode;
using UnityEngine;

public class PlayerInteraction : NetworkBehaviour
{
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E키 누름");
            Collider[] cols = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider col in cols)
            {
                if (col.CompareTag("Ore"))
                {
                    NetworkObject ore = col.GetComponent<NetworkObject>();
                    GetOreRpc(ore);
                }
            }
        }
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
