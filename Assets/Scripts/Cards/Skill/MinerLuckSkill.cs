using UnityEngine;

[CreateAssetMenu(menuName = "Skill/MinerLuck")]
public class MinerLuckSkill : CardSkillBase
{
    public int chance = 10;

    public override void Use(PlayerStatus user, PlayerStatus target)
    {
        user.minerLuckChance = chance;

        Debug.Log("광부의 행운 적용 : " + chance + "%");
    }
}