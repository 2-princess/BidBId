using UnityEngine;

public enum CardSkillType
{
    PickPocket,
    Slave,
    MinerLuck,
    OverTimeOrder,
}
public enum CardGrade
{
    Common,
    Rare,
    Epic
}
[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public CardSkillBase skill;

    public int id;
    public string cardName;
    public CardSkillType skillType;
    public CardGrade grade;

    [TextArea]
    public string skillDescription;

    public bool needTarget;
    public Sprite icon;
}
