using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{
    public void GoToCamp()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.NonSpecific);
    }

    public void GoToCheckpoint()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenu()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
    }
}
