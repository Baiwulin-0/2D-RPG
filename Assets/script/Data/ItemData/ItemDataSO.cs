using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    [Header("Merchant details")]
    [Range(0, 1000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 100;


    [Header("Item details")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;


    [Header("Item efect")]
    public ItemEffect_DataSO itemEffect;

    [Header("craft details")]
    public Inventory_Item[] craftRecipe;
}
