using UnityEngine;

public class LineManager : MonoBehaviour
{
    private SpringJoint2D springJointToWatch;
    private Attach_Goo parentGoo;
    private LineRenderer lineRenderer;

    // Method to set which SpringJoint2D this LineRenderer will monitor
    public void SetSpringJointToWatch(SpringJoint2D springJoint)
    {
        springJointToWatch = springJoint;
    }

    private void Start()
    {
        // Find the parent Attach_Goo script
        parentGoo = GetComponentInParent<Attach_Goo>();
        lineRenderer = GetComponent<LineRenderer>();

        if (parentGoo == null || springJointToWatch == null || lineRenderer == null)
        {
            Debug.LogError("Parent Attach_Goo, LineRenderer, or specific SpringJoint2D not set.");
        }
    }

    private void Update()
    {
        // If the watched SpringJoint2D is disabled or the connectedBody is null, destroy the LineRenderer
        if (springJointToWatch != null && (!springJointToWatch.enabled || springJointToWatch.connectedBody == null))
        {
            // Only destroy the LineRenderer if neither the Goo is a Start or End Goo
            if (!parentGoo._IsGooStart && !parentGoo._IsGooEnd)
            {
                Destroy(gameObject); // Destroy the LineRenderer GameObject
                Debug.Log("LineRenderer destroyed due to SpringJoint2D disconnection or deactivation.");
            }
        }
    }
}