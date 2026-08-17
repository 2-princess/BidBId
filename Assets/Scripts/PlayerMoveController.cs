using Unity.Netcode;
using UnityEngine;

public class PlayerMoveController : NetworkBehaviour
{
    public Rigidbody playerRigid;
    public Animator animator;
    public Transform skull;
    public bool isGround = false;
    float speed = 4f;
    public PlayerState state;
    public enum PlayerState
    {
        Idle,
        Walk,
        Run
    }

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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 6f;
                state = PlayerState.Run;
            }
            else
            {
                speed = 4f;
                state = PlayerState.Walk;
            }
        }
        else
        {
            state = PlayerState.Idle;
        }
        ani(state);
    }

    void ani(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                break;
            case PlayerState.Walk:
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
                break;
            case PlayerState.Run:
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
                break;

        }
    }
}
