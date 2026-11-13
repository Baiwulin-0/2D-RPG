using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : MonoBehaviour
{
    private UI ui;
    private DialogueNpcData npcData;
    private Player_QuestManager questmanager;

    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI[] dialogueChoicesText;

    [Space]
    [SerializeField] private float textSpeed = .1f;
    private string fullTextToShow;
    private Coroutine typeTextCo;

    private DialogueLineSO currentLine;
    private DialogueLineSO[] currentChoices;
    private DialogueLineSO selectedChoice;
    private int selectedChoiceIndex;
    private bool waitingToConfirm;
    private bool canInteract;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        questmanager = Player.instance.questManager;
    }

    public void SetupNpcData(DialogueNpcData npcData) => this.npcData = npcData;


    public void PlayDialogueLine(DialogueLineSO line)
    {
        if (line == null)
            return;

        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            typeTextCo = null;
        }

        currentLine = line;
        currentChoices = line.choiceLines;
        canInteract = false;
        waitingToConfirm = false;      // 每条新台词都重置，避免上一条的状态漏到这一条
        selectedChoice = null;
        selectedChoiceIndex = 0;

        HideAllChoices();

        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        bool useRandomLine = line.actionType == DialogueActionType.None || line.actionType == DialogueActionType.PlayerMakeChoice;

        if (useRandomLine && line.textLine != null && line.textLine.Length > 0)
            fullTextToShow = line.GetRandomLine();
        else if (string.IsNullOrEmpty(line.actionLine) == false)
            fullTextToShow = line.actionLine;
        else if (line.textLine != null && line.textLine.Length > 0)
            fullTextToShow = line.GetFirstLine();      // actionLine 没填时退回头一句台词
        else
            fullTextToShow = "";

        typeTextCo = StartCoroutine(TypeTextCo(fullTextToShow));
        StartCoroutine(EnableInteractionCo());
    }

    private void HandleNextAction()
    {
        if (currentLine == null)
            return;

        switch (currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToInGameUI();
                ui.OpenMerchantUI(true);
                break;

            case DialogueActionType.OpenQuest:
                ui.SwitchToInGameUI();
                ui.OpenQuestUI(npcData.quests);
                break;

            case DialogueActionType.GetQuestReward:
                ui.SwitchToInGameUI();
                questmanager.TryGetRewardFrom(npcData.npcRewardType);
                break;

            case DialogueActionType.CloseDialogue:
                ui.SwitchToInGameUI();
                break;

            case DialogueActionType.PlayerMakeChoice:
                if (currentChoices == null || currentChoices.Length == 0)
                {
                    Debug.LogWarning($"[Dialogue] \"{currentLine.name}\" 的 actionType 是 PlayerMakeChoice，" +
                                     "但 choiceLines 是空的。请把这个资源的 actionType 改成 OpenQuest / OpenShop / CloseDialogue，或者补上选项。", currentLine);
                    ui.SwitchToInGameUI();
                    break;
                }

                if (selectedChoice == null)
                {
                    ShowChoices();
                }
                else
                {
                    // 注意：这里原来写的是 “DialogueLineSO selectedChoice = ...”，
                    // 声明了一个同名局部变量把字段遮住了，字段根本没有被清掉。
                    DialogueLineSO choice = currentChoices[selectedChoiceIndex];

                    selectedChoice = null;
                    waitingToConfirm = false;
                    PlayDialogueLine(choice);
                }
                break;

            case DialogueActionType.None:
            case DialogueActionType.OpenCraft:
            default:
                // 普通台词 / 还没实现的动作：什么都不做，别把对话框卡死在这里
                break;
        }
    }

    private void CompleteTyping()
    {
        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text = fullTextToShow;
            typeTextCo = null;
        }
    }

    private bool ChoiceIsSelectable(int index)
    {
        if (currentChoices == null || index < 0 || index >= currentChoices.Length)
            return false;

        DialogueLineSO choice = currentChoices[index];
        if (choice == null)
            return false;

        // 没有“这个 NPC 能发的奖励”时，“领取奖励”选项不显示
        if (choice.actionType == DialogueActionType.GetQuestReward && HasRewardToClaim() == false)
            return false;

        return true;
    }

    private bool HasRewardToClaim()
    {
        if (questmanager == null)
            return false;

        if (npcData == null)
            return questmanager.HasCompletedQuest();

        return questmanager.HasCompletedQuest(npcData.npcRewardType);
    }

    private void ShowChoices()
    {
        if (currentChoices == null || currentChoices.Length == 0)
            return;

        // 如果当前下标指向一个不可选的选项，往后找一个能选的
        if (ChoiceIsSelectable(selectedChoiceIndex) == false)
        {
            int start = selectedChoiceIndex;

            do
            {
                selectedChoiceIndex = (selectedChoiceIndex + 1) % currentChoices.Length;
            }
            while (ChoiceIsSelectable(selectedChoiceIndex) == false && selectedChoiceIndex != start);
        }

        selectedChoice = currentChoices[selectedChoiceIndex];

        if (dialogueChoicesText == null || dialogueChoicesText.Length == 0)
        {
            // UI 上没配选项文本框：流程还能走（用 W/S 选、F 确认），但玩家看不到选项
            waitingToConfirm = true;
            return;
        }

        // 只配了一个文本框时，把所有选项拼成多行显示，选中项高亮
        if (dialogueChoicesText.Length == 1 && dialogueChoicesText[0] != null)
        {
            string block = "";

            for (int i = 0; i < currentChoices.Length; i++)
            {
                if (ChoiceIsSelectable(i) == false)
                    continue;

                string line = $"{i + 1}) {currentChoices[i].playerChoiceAnswer}";
                block += (i == selectedChoiceIndex ? $"<color=yellow>{line}</color>" : line) + "\n";
            }

            dialogueChoicesText[0].gameObject.SetActive(true);
            dialogueChoicesText[0].text = block.TrimEnd();
            waitingToConfirm = true;
            return;
        }

        for (int i = 0; i < dialogueChoicesText.Length; i++)
        {
            if (dialogueChoicesText[i] == null)
                continue;

            if (i >= currentChoices.Length || ChoiceIsSelectable(i) == false)
            {
                dialogueChoicesText[i].gameObject.SetActive(false);
                continue;
            }

            string choiceText = currentChoices[i].playerChoiceAnswer;

            dialogueChoicesText[i].gameObject.SetActive(true);
            dialogueChoicesText[i].text = i == selectedChoiceIndex
                ? $"<color=yellow>{i + 1}) {choiceText}</color>"
                : $"{i + 1}) {choiceText}";
        }

        // ★ 关键：选项已经显示出来了，接下来按 F 应该“确认选项”，而不是什么都不做
        waitingToConfirm = true;
    }

    private void HideAllChoices()
    {
        if (dialogueChoicesText == null)
            return;

        foreach (var obj in dialogueChoicesText)
        {
            if (obj != null)
                obj.gameObject.SetActive(false);
        }
    }

    public void NavigateChoice(int direction)
    {
        if (currentChoices == null || currentChoices.Length <= 1)
            return;

        direction = direction >= 0 ? 1 : -1;
        int index = selectedChoiceIndex;

        for (int i = 0; i < currentChoices.Length - 1; i++)
        {
            index = Mathf.Clamp(index + direction, 0, currentChoices.Length - 1);

            if (ChoiceIsSelectable(index))
            {
                if (index != selectedChoiceIndex)
                {
                    selectedChoiceIndex = index;
                    ShowChoices();
                }
                return;
            }

            if (index == 0 || index == currentChoices.Length - 1)
                return;
        }
    }

    public void DialogueInteraction()
    {
        if (canInteract == false)
            return;

        if (typeTextCo != null)
        {
            CompleteTyping();

            if (currentLine.actionType != DialogueActionType.PlayerMakeChoice)
                waitingToConfirm = true;
            else
                HandleNextAction();

            return;
        }

        if (waitingToConfirm)
        {
            waitingToConfirm = false;
            HandleNextAction();
        }
    }

    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text = dialogueText.text + letter;
            yield return new WaitForSeconds(textSpeed);
        }

        if (currentLine.actionType != DialogueActionType.PlayerMakeChoice)
        {
            waitingToConfirm = true;
        }
        else
        {
            yield return new WaitForSeconds(.2f);
            selectedChoice = null;
            HandleNextAction();
        }

        typeTextCo = null;
    }

    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract = true;
    }

}
