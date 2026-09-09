using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using static PlayerAnimationController;

public class PlayerMoveController : NetworkBehaviour
{
    public Rigidbody playerRigid;

    public Transform skull;
    public bool isGround = false;
    public bool isMove = true;
    public PlayerAnimationController aniCon;
    private bool isCallMove = false;
    private float speed = 3f;
    [SerializeField] private PlayerStatusEffectController statusEffect;
    [SerializeField] private NavMeshAgent agent;


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
        if (isCallMove)
        {
            agent.nextPosition = transform.position;

            Vector3 dir = agent.desiredVelocity;

            playerRigid.linearVelocity = new Vector3(dir.x, playerRigid.linearVelocity.y, dir.z);
            skull.LookAt(skull.position + dir);

            // 목적지 도착
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
            {
                agent.ResetPath();

                isCallMove = false;
                isMove = true;

                playerRigid.linearVelocity = new Vector3(0, playerRigid.linearVelocity.y, 0);
                aniCon.SetAni(PlayerState.Idle);
            }
            return;
        }
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
        Vector3 moving = new Vector3(moveDir.x * speed * statusEffect.MoveSpeedMultiplier, y, moveDir.z * speed * statusEffect.MoveSpeedMultiplier);
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
        SetMove(false);
        // 착지 순간 좌우 이동도 잠깐 멈춤
        playerRigid.linearVelocity = new Vector3(0, playerRigid.linearVelocity.y, 0);
        yield return new WaitForSeconds(0.3f);
        SetMove(true);
    }


    public void StartCallMove(Vector3 destination)
    {
        if (!IsServer) return;
        Debug.Log("Agent : " + agent);
        Debug.Log("NavMesh 위인가 : " + agent.isOnNavMesh);
        Debug.Log("목적지 : " + destination);
        StartCallMoveRpc(destination);
    }

    [Rpc(SendTo.Owner)]
    private void StartCallMoveRpc(Vector3 destination)
    {
        isMove = false;
        isCallMove = true;

        agent.nextPosition = transform.position;
        agent.SetDestination(destination);

        aniCon.SetAni(PlayerState.Run);
    }

    public void SetMove(bool value)
    {
        SetMoveRpc(value);
    }

    [Rpc(SendTo.Owner)]
    private void SetMoveRpc(bool value)
    {
        isMove = value;
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
