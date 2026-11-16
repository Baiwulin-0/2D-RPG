using UnityEngine;

public class CircleDetector : MonoBehaviour, IDetectionArea
{
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask whatIsTarget;

    public Collider2D[] GetDetectedColliders()
        => Physics2D.OverlapCircleAll(targetCheck.position, radius, whatIsTarget);

    private void OnDrawGizmos()
    {
        if (targetCheck == null) return;
        Gizmos.DrawWireSphere(targetCheck.position, radius);
    }
}
