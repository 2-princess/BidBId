using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerState state;
    public Animator animator;

    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Randing,
        Mining
    }

    public void SetAni(PlayerState newState)
    {
        if (state == newState) return;

        state = newState;
        ani(state);
    }

    void ani(PlayerState state)
    {
        ResetAni();
        switch (state)
        {
            case PlayerState.Mining:
                animator.SetBool("Mining", true);
                break;
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
    public void SetMiningSpeed(float speed)
    {
        animator.SetFloat("MiningSpeed", speed);
    }
    void ResetAni()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Fall", false);
        animator.SetBool("Randing", false);
        animator.SetBool("Mining", false);
    }
}
