using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attach_Goo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int RadiusDetection;
    [SerializeField] private SpringJoint2D[] springJoints;
    [SerializeField] private GameObject LinePreafab;
    [SerializeField] private bool isGooStart;


    [SerializeField] private bool isGooEnd;
    [SerializeField] private float connectionRange = 5f;
    private List<GameObject> LinePreafabList = new List<GameObject>();
    private List<GameObject> RbConnected = new List<GameObject>();



    public bool _IsGooStart => isGooStart;
    public bool _IsGooEnd => isGooEnd;
    public List<GameObject> _LinePreafabList => LinePreafabList;
    public List<GameObject> _RbConnected => RbConnected;

    [SerializeField] private GooType GooType;

    public GooType _GooType
    {
        get { return GooType; }
    }

    public static Attach_Goo selectedGoo1;
    private Rigidbody2D rb2d;
    private int JointCount = 0;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        springJoints = GetComponents<SpringJoint2D>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click attach Goo");
        if (CompareTag("Goo"))
        {
            if (selectedGoo1 == null)
            {
                selectedGoo1 = this;
                Debug.Log("First object selected: " + gameObject.name);
            }
            else if (selectedGoo1 != this)
            {
                if (selectedGoo1.JointCount < 2)
                {
                    CreateSpringJoint(selectedGoo1, this);
                }
                else if (JointCount < 2)
                {
                    CreateSpringJoint(this, selectedGoo1);
                }

                selectedGoo1 = null;
            }
        }
    }

    private void CreateSpringJoint(Attach_Goo goo1, Attach_Goo goo2)
    {
        // Block the connection between two Start Goo objects
        if (goo1.isGooStart && goo2.isGooStart)
        {
            Debug.Log("Cannot connect two Goo Starts together.");
            return;
        }

        float distance = Vector2.Distance(goo1.transform.position, goo2.transform.position);

        // Check if the selected Goo is in range
        if (distance <= connectionRange)
        {
            SpringJoint2D availableJoint = null;

            // EndGoo creates a joint automatically with the selected Goo
            if (goo1.isGooEnd || goo2.isGooEnd)
            {
                // If none of the Goos is an EndGoo, cancel the initiation
                if (!goo1.isGooEnd && !goo2.isGooEnd)
                {
                    Debug.Log("Connection must be initiated by a Goo End.");
                    return;
                }

                foreach (var joint in goo2.springJoints)
                {
                    if (joint.connectedBody == null)
                    {
                        availableJoint = joint;
                        break;
                    }
                }

                if (availableJoint != null)
                {
                    CreateLineRenderer(goo1, goo2, availableJoint);
                    availableJoint.connectedBody = goo1.rb2d;
                    availableJoint.autoConfigureConnectedAnchor = false;
                    availableJoint.connectedAnchor = Vector2.zero;
                    goo1.JointCount++;
                }
            }

            // If Goo1 is a Start or End Goo, connect it to a regular Goo
            if (goo1.isGooStart || goo1.isGooEnd)
            {
                foreach (var joint in goo2.springJoints)
                {
                    if (joint.connectedBody == null)
                    {
                        availableJoint = joint;
                        break;
                    }
                }

                if (availableJoint != null)
                {
                    CreateLineRenderer(goo1, goo2, availableJoint);
                    availableJoint.connectedBody = goo1.rb2d;
                    availableJoint.autoConfigureConnectedAnchor = false;
                    availableJoint.connectedAnchor = Vector2.zero;
                    goo1.JointCount++;
                }
            }
            // Same logic but in reverse order
            else if (goo2.isGooStart || goo2.isGooEnd)
            {
                foreach (var joint in goo1.springJoints)
                {
                    if (joint.connectedBody == null)
                    {
                        availableJoint = joint;
                        break;
                    }
                }

                if (availableJoint != null)
                {
                    CreateLineRenderer(goo1, goo2, availableJoint);
                    availableJoint.connectedBody = goo2.rb2d;
                    availableJoint.autoConfigureConnectedAnchor = false;
                    availableJoint.connectedAnchor = Vector2.zero;
                    goo1.JointCount++;
                    goo2.JointCount++;
                }
            }
            else
            {
                // Both Goo1 and Goo2 are not Start or End Goos
                foreach (var joint in goo1.springJoints)
                {
                    if (joint.connectedBody == null)
                    {
                        availableJoint = joint;
                        break;
                    }
                }

                if (availableJoint != null)
                {
                    CreateLineRenderer(goo1, goo2, availableJoint);
                    availableJoint.connectedBody = goo2.rb2d;
                    availableJoint.autoConfigureConnectedAnchor = false;
                    availableJoint.connectedAnchor = Vector2.zero;
                    goo1.JointCount++;
                    goo2.JointCount++;
                }
            }

            Debug.Log("Connection established between " + goo1.gameObject.name + " and " + goo2.gameObject.name);
        }
        else
        {
            Debug.Log("Goo's are too far from each other.");
        }
    }

    private void CreateLineRenderer(Attach_Goo goo1, Attach_Goo goo2, SpringJoint2D springJoint)
    {
        // Create a new GameObject with the LineRenderer component
        GameObject _line = Instantiate(LinePreafab, goo1.transform.position, Quaternion.identity, goo1.transform);
        LineRenderer _lineRenderer = _line.GetComponent<LineRenderer>();

        // Set positions of Goo1 and Goo2 for the start and end of the LineRenderer
        _lineRenderer.SetPosition(0, goo1.transform.position);
        _lineRenderer.SetPosition(1, goo2.transform.position);

        // Associate the correct SpringJoint with the LineManager
        LineManager lineManager = _line.GetComponent<LineManager>();
        lineManager.SetSpringJointToWatch(springJoint);

        // Add the connected Rigidbody and LineRenderer to the lists
        RbConnected.Add(goo2.gameObject);
        LinePreafabList.Add(_line);
    }


    private void Update()
    {
        if (gameObject.activeSelf)
        {
            // Update the positions of the LineRenderer.
            for (int i = 0; i < LinePreafabList.Count; i++)
            {
                if (LinePreafabList[i] != null)
                {
                    LinePreafabList[i].GetComponent<LineRenderer>()
                        .SetPosition(0, LinePreafabList[i].transform.parent.position);
                    LinePreafabList[i].GetComponent<LineRenderer>().SetPosition(1, RbConnected[i].transform.position);
                }
            }

            // Detect if a SpringJoint2D is disconnected or disabled and destroy the corresponding LineRenderer.
            for (int i = 0; i < springJoints.Length; i++)
            {
                // Check if the SpringJoint is disabled or disconnected
                if (!springJoints[i].enabled || springJoints[i].connectedBody == null)
                {
                    // Only destroy LineRenderer if neither Goo is a Start or End Goo
                    if (!isGooStart && !isGooEnd)
                    {
                        // Find the index of the connected GameObject and destroy the corresponding LineRenderer
                        int index = RbConnected.FindIndex(rb => rb == springJoints[i].connectedBody?.gameObject);

                        if (index >= 0 && LinePreafabList[index] != null)
                        {
                            Destroy(LinePreafabList[index]); // Destroy the LineRenderer
                            LinePreafabList.RemoveAt(index); // Remove from the list
                            RbConnected.RemoveAt(index);    // Remove from the connected Rigidbody list
                        }

                        Debug.Log("SpringJoint disconnected or disabled, LineRenderer destroyed.");
                    }
                }
            }
        }
    }

}
