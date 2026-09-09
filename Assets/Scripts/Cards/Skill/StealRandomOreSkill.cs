using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/StealRandomOre")]
public class StealRandomOreSkill : CardSkillBase
{
    public override void Use(PlayerStatus user, PlayerStatus target)
    {
        if (target == null) return;

        PlayerInventory userInventory = user.GetComponent<PlayerInventory>();

        PlayerInventory targetInventory = target.GetComponent<PlayerInventory>();

        if (userInventory == null || targetInventory == null)
            return;

        List<int> ownedOres = new List<int>();

        foreach (InventorySlot slot in targetInventory.inventory)
        {
            if (slot.count <= 0) continue;

            ItemId item = (ItemId)slot.itemId;

            if (item == ItemId.Iron ||
                item == ItemId.Copper ||
                item == ItemId.Gold ||
                item == ItemId.Diamond)
            {
                ownedOres.Add(slot.itemId);
            }
        }

        if (ownedOres.Count == 0)
        {
            Debug.Log("상대가 가진 광물이 없음");
            return;
        }

        int randomIndex = Random.Range(0, ownedOres.Count);
        int stolenItemId = ownedOres[randomIndex];

        if (targetInventory.RemoveItem(stolenItemId, 1))
        {
            userInventory.AddItem(stolenItemId, 1);

            Debug.Log(
                "광물 강탈 성공 : " + (ItemId)stolenItemId
            );
        }
    }
}