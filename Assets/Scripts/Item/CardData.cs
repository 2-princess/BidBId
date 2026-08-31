using UnityEngine;

public enum CardGrade
{
    Common,
    Rare,
    Epic,
    Legendary
}
[CreateAssetMenu(menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public int id;
    public string cardName;
    public CardGrade grade;

    public string skillName;
    [TextArea]
    public string skillDescription;

    public Sprite icon;
}
