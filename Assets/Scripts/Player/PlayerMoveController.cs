using Unity.Netcode;
using UnityEngine;
using static PlayerAnimationController;

public class PlayerMoveController : NetworkBehaviour
{
    public Rigidbody playerRigid;

    public Transform skull;
    public bool isGround = false;
    public bool isMove = true;
    public PlayerAnimationController aniCon;
    float speed = 4f;

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.1f)
            {
                isGround = true;
                break;
            }
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        if(!isMove) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * v + right * h;
        Vector3 moving = new Vector3(moveDir.x * speed, playerRigid.linearVelocity.y, moveDir.z * speed);

        playerRigid.linearVelocity = moving;

        if (h != 0 || v != 0)
        {
            Vector3 lookDir = new Vector3(moving.x, 0, moving.z);
            skull.LookAt(skull.position + lookDir);
        }
        if (!isGround) // 점프
        {
            if (playerRigid.linearVelocity.y > 0)
            {
                aniCon.SetAni(PlayerState.Jump);
            }
            else
            {
                if (Physics.Raycast(transform.position, Vector3.down, 1f))
                {
                    aniCon.SetAni(PlayerState.Randing);
                }
                else
                {
                    aniCon.SetAni(PlayerState.Fall);
                }
            }
        }
        else // 걷기,달리기
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
                    aniCon.SetAni(PlayerState.Run);
                }
                else
                {
                    speed = 4f;
                    aniCon.SetAni(PlayerState.Walk);
                }
            }
            else
            {
                aniCon.SetAni(PlayerState.Idle);
            }
        }
    }
}
