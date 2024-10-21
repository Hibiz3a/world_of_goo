using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attach_Goo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int RadiusDetection;
    [SerializeField] private SpringJoint2D[] springJoints;
    [SerializeField] private GameObject LinePrefab;
    [SerializeField] private bool isGooStart;
    [SerializeField] private bool isGooEnd;
    [SerializeField] private float connectionRange = 5f;
    [SerializeField] private GooType GooType;
    private List<GameObject> LinePrefabList = new List<GameObject>();
    private List<GameObject> RbConnected = new List<GameObject>();
    private Rigidbody2D rb2d;
    private int JointCount = 0;


    public static Attach_Goo selectedGoo1;
    public bool _IsGooStart => isGooStart;
    public bool _IsGooEnd => isGooEnd;
    public List<GameObject> _LinePrefabList => LinePrefabList;
    public List<GameObject> _RbConnected => RbConnected;
    public int _JointCount => JointCount;


    public GooType _GooType
    {
        get { return GooType; }
    }


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        springJoints = GetComponents<SpringJoint2D>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click attach Goo");

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
        if (goo1.isGooStart && goo2.isGooStart)
        {
            Debug.Log("Cannot connect two Goo Starts together.");
            return;
        }

        float distance = Vector2.Distance(goo1.transform.position, goo2.transform.position);

        if (distance <= connectionRange)
        {
            SpringJoint2D availableJoint = null;

            if (goo1.isGooEnd || goo2.isGooEnd)
            {
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
            Debug.Log("Goos are too far from each other.");
        }
    }

    private void CreateLineRenderer(Attach_Goo goo1, Attach_Goo goo2, SpringJoint2D springJoint)
    {
        GameObject _line = Instantiate(LinePrefab, goo1.transform.position, Quaternion.identity, goo1.transform);
        LineRenderer _lineRenderer = _line.GetComponent<LineRenderer>();

        _lineRenderer.SetPosition(0, goo1.transform.position);
        _lineRenderer.SetPosition(1, goo2.transform.position);

        LineManager lineManager = _line.GetComponent<LineManager>();
        lineManager.SetSpringJointToWatch(springJoint);

        RbConnected.Add(goo2.gameObject);
        LinePrefabList.Add(_line);
    }

    private void Update()
    {
        for (int i = 0; i < LinePrefabList.Count; i++)
        {
            if (LinePrefabList[i] != null)
            {
                LinePrefabList[i].GetComponent<LineRenderer>()
                    .SetPosition(0, LinePrefabList[i].transform.parent.position);
                LinePrefabList[i].GetComponent<LineRenderer>().SetPosition(1, RbConnected[i].transform.position);
            }
        }

        for (int i = 0; i < springJoints.Length; i++)
        {
            if (!springJoints[i].enabled || springJoints[i].connectedBody == null)
            {
                if (!isGooStart && !isGooEnd)
                {
                    int index = RbConnected.FindIndex(rb => rb == springJoints[i].connectedBody?.gameObject);

                    if (index >= 0 && LinePrefabList[index] != null)
                    {
                        Destroy(LinePrefabList[index]);
                        LinePrefabList.RemoveAt(index);
                        RbConnected.RemoveAt(index);
                    }

                    Debug.Log("SpringJoint disconnected or disabled, LineRenderer destroyed.");
                }
            }
        }
    }
}