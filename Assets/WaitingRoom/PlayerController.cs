using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    float speed = 4f;

    void Update()
    {
        if (!IsOwner) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moving = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.position += moving;
    }
}
