using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Quest : MonoBehaviour,ISaveable
{
    private GameData currentGameData;

    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_QuestPreviw questPreviw;
    private UI_QuestSlot[] questSlots;
    public Player_QuestManager questManager {  get; private set; }

    private void Awake()
    {
        questSlots = GetComponentsInChildren<UI_QuestSlot>(true);
        questManager = Player.instance.questManager;
    }

    public void SetupQuestUI(QuestDataSO[] questsToSetup)
    {
        // 1. 先把所有的 UI 槽位隐藏
        // 使用 questSlots 数组，并且判空防止意外
        foreach (var slot in questSlots)
        {
            if (slot != null)
                slot.gameObject.SetActive(false);
        }

        // 2. 再根据任务数据，开启对应的槽位并赋值
        for (int i = 0; i < questsToSetup.Length; i++)
        {
            // 防止任务数据数量超过 UI 槽位数量导致越界
            if (i >= questSlots.Length) break;

            questSlots[i].gameObject.SetActive(true);
            questSlots[i].SetupQuestSlot(questsToSetup[i]);
        }

        questPreviw.MakeQuestPreviwEmpty();
        inventorySlots.UpdateSlots(Player.instance.inventory.itemList);

        UpdateQuestList();
    }

    public void UpdateQuestList()
    {
        foreach (var slot in questSlots)
        {
            if (slot.questInSlot == null) continue;

            if (slot.gameObject.activeSelf && CanTakeQuest(slot.questInSlot) == false)
                slot.gameObject.SetActive(false);
        }
    }

    private bool CanTakeQuest(QuestDataSO questToCheck)
    {
        bool questActive = questManager.QuestIsActive(questToCheck);

        if (currentGameData != null)
        {
            bool questCompleted = 
                currentGameData.completeQuests.TryGetValue(questToCheck.questSaveId,out bool isCompleted) && isCompleted;

            return questActive == false && questCompleted == false;
        }

        return questActive == false;
    }

    public UI_QuestPreviw GetQuestPreviw() => questPreviw;

    public void LoadData(GameData data)
    {
        currentGameData = data;
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
