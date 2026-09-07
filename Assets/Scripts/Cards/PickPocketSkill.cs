using UnityEngine;

[CreateAssetMenu(menuName = "Skill/PickPocket")]
public class PickPocketSkill : CardSkillBase
{
    public int stealPercent;

    public override void Use(PlayerStatus user, PlayerStatus target)
    {
        int goldMinus = Mathf.FloorToInt(target.gold.Value * stealPercent / 100f);

        target.RemoveGold(goldMinus);
        user.AddGold(goldMinus);

        Debug.Log("소매치기 성공 : " + goldMinus + "G");
    }
}