using Unity.VisualScripting;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private Vector2 offset = new Vector2(0,0);

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform targetRect)
    {
        if(show == false)
        {
            rect.position = new Vector2(9999, 9999);
            return ;
        }

        UpdatePosition(targetRect);
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2f;
        float screenCenterY = Screen.height / 2f;

        Vector2 targetPosition = targetRect.position;

        targetPosition.x = targetPosition.x >screenCenterX ? targetPosition.x - offset.x:targetPosition.x + offset.x;
        targetPosition.y = targetPosition.y >screenCenterY ? targetPosition.y + offset.y:targetPosition.y - offset.y;

        rect.position = targetPosition;
    }

    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }
}
