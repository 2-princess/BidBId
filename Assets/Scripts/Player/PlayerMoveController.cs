using System.Collections;
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
    float speed = 3f;

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.1f)
            {
                isGround = true;
                StartCoroutine(LandingDelay());
                break;
            }
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        if (!isMove) return;
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
        float y = playerRigid.linearVelocity.y;

        if (isGround && y > 0) { y = 0; }
        Vector3 moving = new Vector3(moveDir.x * speed, y, moveDir.z * speed);
        playerRigid.linearVelocity = moving;

        if (moveDir != Vector3.zero)
        {
            skull.LookAt(skull.position + moveDir);
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
                    speed = 5f;
                    aniCon.SetAni(PlayerState.Run);
                }
                else
                {
                    speed = 3f;
                    aniCon.SetAni(PlayerState.Walk);
                }
            }
            else
            {
                aniCon.SetAni(PlayerState.Idle);
            }
        }
    }
    IEnumerator LandingDelay()
    {
        isMove = false;
        // 착지 순간 좌우 이동도 잠깐 멈춤
        playerRigid.linearVelocity = new Vector3(0, playerRigid.linearVelocity.y, 0);
        yield return new WaitForSeconds(0.35f);

        isMove = true;
    }

    [Rpc(SendTo.Owner)]
    public void SpawnTeleportRpc(Vector3 spawnPosition)
    {
        Debug.Log("RPC 받은 위치 : " + spawnPosition);

        playerRigid.linearVelocity = Vector3.zero;

        playerRigid.position = spawnPosition;

        Debug.Log("이동 직후 위치 : " + transform.position);
    }
}
