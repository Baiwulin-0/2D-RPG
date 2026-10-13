using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviw : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;
    private UI_CraftPreviwSlot[] craftPreviwSlots;

    [Header("Item Previw Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;


    public void SetupCraftPreviw(Inventory_Storage storage)
    {
        this.storage = storage;

        craftPreviwSlots = GetComponentsInChildren<UI_CraftPreviwSlot>();
        foreach(var slot in craftPreviwSlots)
            slot.gameObject.SetActive(false);
    }

    public void ConfirmCraft()
    {
        if (itemToCraft == null)
        {
            buttonText.text = "选择一个物品！";
            return;
        }

        if (storage.CanCraftItem(itemToCraft))
        {
            storage.CraftItem(itemToCraft);
        }

        UpdateCraftPreviwSlots();
    }

    public void UpdateCraftPreviw(ItemDataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();
        UpdateCraftPreviwSlots();

    }

    private void UpdateCraftPreviwSlots()
    {
        foreach (var slot in craftPreviwSlots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];
            int availableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = requiredItem.stackSize;

            craftPreviwSlots[i].gameObject.SetActive(true);
            craftPreviwSlots[i].SetupPreviwSlot(requiredItem.itemData, availableAmount, requiredAmount);

        }
    }
}
