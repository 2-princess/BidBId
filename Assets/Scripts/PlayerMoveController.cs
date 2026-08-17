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
        Run,
        Jump,
        Fall,
        Randing
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moving = new Vector3(h * speed, playerRigid.linearVelocity.y, v * speed);
        playerRigid.linearVelocity = moving;
        if (h != 0 || v != 0)
        {
            Vector3 lookDir = new Vector3(moving.x, 0, moving.z);
            skull.LookAt(skull.position + lookDir);
        }
        if (!isGround)
        {
            if (playerRigid.linearVelocity.y > 0)
            {
                state = PlayerState.Jump;
            }
            else
            {
                if (Physics.Raycast(transform.position, Vector3.down, 1f))
                {
                    state = PlayerState.Randing;
                }
                else
                {
                    state = PlayerState.Fall;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                playerRigid.AddForce(Vector3.up * 7f, ForceMode.Impulse);
                isGround = false;
            }
            if (h != 0 || v != 0)
            {
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
        }
        ani(state);
    }

    void ani(PlayerState state)
    {
        ResetAni();
        switch (state)
        {
            case PlayerState.Walk:
                animator.SetBool("Walk", true);
                break;
            case PlayerState.Run:
                animator.SetBool("Run", true);
                break;
            case PlayerState.Jump:
                animator.SetBool("Jump", true);
                break;
            case PlayerState.Fall:
                animator.SetBool("Fall", true);
                break;
            case PlayerState.Randing:
                animator.SetBool("Randing", true);
                break;
        }
    }
    void ResetAni()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);
        animator.SetBool("Randing", false);
    }
}
