using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Portal Scroll", fileName = "Item effect data - PortalScroll")]
public class ItemEffect_PortalScroll : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        if (SceneManager.GetActiveScene().name == "Level_0")
        {
            Debug.Log("不能在城里打开传送卷轴");
            return;
        }

        Player player = Player.instance;
        Vector3 portalPosition = player.transform.position + new Vector3(player.facingDir * 1.5f, -1.7f);

        Object_Portal.instance.ActivatePortal(portalPosition, player.facingDir);
    }
}
