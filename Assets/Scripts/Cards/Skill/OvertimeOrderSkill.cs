using UnityEngine;

[CreateAssetMenu(menuName = "Skill/OvertimeOrder")]
public class OvertimeOrderSkill : CardSkillBase
{
    public float speedMultiplier = 0.5f;

    public override void Use(PlayerStatus user, PlayerStatus target)
    {
        if (target == null) return;

        PlayerStatusEffectController statusEffect = target.GetComponent<PlayerStatusEffectController>();

        if (statusEffect == null) return;

        statusEffect.StartOvertime(speedMultiplier);
    }
}