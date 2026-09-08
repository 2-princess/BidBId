using UnityEngine;

[CreateAssetMenu(menuName = "Skill/CallPlayer")]
public class CallPlayerSkill : CardSkillBase
{
    public override void Use(PlayerStatus user, PlayerStatus target)
    {
        if (target == null) return;

        PlayerMoveController moveController = target.GetComponent<PlayerMoveController>();

        if (moveController == null) return;

        moveController.StartCallMove(user.transform.position);
    }
}
