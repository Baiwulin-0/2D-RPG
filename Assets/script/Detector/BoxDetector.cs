using UnityEngine;

public class BoxDetector : MonoBehaviour, IDetectionArea
{
    [SerializeField] private Transform targetCheck;
    [SerializeField] private Vector2 size = new Vector2(2f, 2f);
    [SerializeField] private float angle = 0f;
    [SerializeField] private LayerMask whatIsTarget;

    public Collider2D[] GetDetectedColliders()
        => Physics2D.OverlapBoxAll(targetCheck.position, size, angle, whatIsTarget);

    private void OnDrawGizmos()
    {
        if (targetCheck == null) return;
        Gizmos.matrix = Matrix4x4.TRS(targetCheck.position, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, size.y, 0f));
        Gizmos.matrix = Matrix4x4.identity;
    }
}
