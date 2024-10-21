using UnityEngine;

public class LineRendererPreview : MonoBehaviour
{
    public Attach_Goo gooA;    // Référence au premier Goo
    public Attach_Goo gooB;    // Référence au deuxième Goo (celui avec lequel on crée la ligne)
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            UpdateLineRendererPositions();
        }
    }

    private void Update()
    {
        UpdateLineRendererPositions();

        if (gooA != null && gooB != null)
        {
            if (gooA._JointCount > 0 || gooB._JointCount > 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void UpdateLineRendererPositions()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, gooA.transform.position);
            lineRenderer.SetPosition(1, gooB.transform.position);
        }
    }
}