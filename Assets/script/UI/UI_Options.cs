using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthBarToggle;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        healthBarToggle.onValueChanged.AddListener(onHealthBarToggleChanged);
    }

    private void onHealthBarToggleChanged(bool isOn)
    {
        player.health.EnableHealthBar(isOn);
    }

    public void GoMainMenuBTN() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
}
