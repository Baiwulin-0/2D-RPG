using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    [SerializeField]private  RespawnType waypointType;
    [SerializeField] private RespawnType conntedWaypoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered = true;

    public RespawnType GetWaypointType() => waypointType;

    public Vector3 GetPositionAndSetTriggerFalse()
    {
        canBeTriggered = false;
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }


    private void OnValidate()
    {
        gameObject.name = "Onject_Waypoint - " + waypointType.ToString() + " - " + transferToScene;

        if (waypointType == RespawnType.Enter)
        {
            conntedWaypoint = RespawnType.Exit;
        }

        if (waypointType == RespawnType.Exit)
        {
            conntedWaypoint = RespawnType.Enter;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggered == false)
        {
            return;
        }
        
        GameManager.instance.ChangeScene(transferToScene, conntedWaypoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}
