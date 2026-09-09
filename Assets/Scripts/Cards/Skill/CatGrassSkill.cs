using UnityEngine;

[CreateAssetMenu(menuName = "Skill/CatGrass")]
public class CatGrassSkill : CardSkillBase
{
    public float duration = 10f;

    public override void Use(PlayerStatus user, PlayerStatus target)
    {
        if (target == null) return;

        PlayerStatusEffectController statusEffect = target.GetComponent<PlayerStatusEffectController>();

        if (statusEffect == null) return;

        statusEffect.StartInterference(duration);
    }
}