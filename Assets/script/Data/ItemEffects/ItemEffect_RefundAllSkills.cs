using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/RefundAllSkills effect", fileName = "Item effect data - Refund All Skills")]
public class ItemEffect_RefundAllSkills : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindAnyObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
