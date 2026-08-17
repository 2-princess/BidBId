using Unity.Netcode;
using UnityEngine;

public class PlayerMoveController : NetworkBehaviour
{
    float speed = 4f;
    public Rigidbody playerRigid;
    public Animator animator;
    public Transform skull;

    void FixedUpdate()
    {
        if (!IsOwner) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moving = new Vector3(h * speed, playerRigid.linearVelocity.y, v * speed);
        playerRigid.linearVelocity = moving;
        if (h != 0 || v != 0)
        {
            Vector3 dir = new Vector3(h, 0, v);
            Quaternion targetRot = Quaternion.LookRotation(dir);
            skull.rotation = targetRot;
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }
}
