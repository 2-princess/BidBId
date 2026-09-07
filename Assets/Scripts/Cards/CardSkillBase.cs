using UnityEngine;

public abstract class CardSkillBase : ScriptableObject
{
    public abstract void Use(PlayerStatus user, PlayerStatus target);
}