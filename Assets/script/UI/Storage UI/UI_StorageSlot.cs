using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;

    public enum StorageSlotType { StorageSlot,PlayerInventorySlot}
    public StorageSlotType SlotType;
    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);

        if (SlotType == StorageSlotType.StorageSlot)
            storage.FormStorageToPlayer(itemInSlot, transferFullStack);

        if(SlotType == StorageSlotType.PlayerInventorySlot)
            storage.FormPlayerToStorage(itemInSlot, transferFullStack);

        ui.itemToolTip.ShowToolTip(false, null);
    }
}
