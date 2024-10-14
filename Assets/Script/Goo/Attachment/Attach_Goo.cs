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
        if (CompareTag("Goo"))
        {
            if (selectedGoo1 == null)
            {
                selectedGoo1 = this;
                Debug.Log("Premier objet sélectionné : " + gameObject.name);
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
        float distance = Vector2.Distance(goo1.transform.position, goo2.transform.position);

        if (distance <= connectionRange)
        {
            SpringJoint2D availableJoint = null;


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
                    CreateLineRenderer(goo2, goo1);
                    availableJoint.connectedBody = goo1.rb2d;
                    availableJoint.autoConfigureConnectedAnchor = false;
                    availableJoint.connectedAnchor = Vector2.zero;
                    goo1.JointCount++;
                    goo2.JointCount++;
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
                    CreateLineRenderer(goo1, goo2);
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
                    CreateLineRenderer(goo1, goo2);
                    availableJoint.connectedBody = goo2.rb2d;
                    availableJoint.autoConfigureConnectedAnchor = false;
                    availableJoint.connectedAnchor = Vector2.zero;
                    goo1.JointCount++;
                    goo2.JointCount++;
                }
            }

            Debug.Log("Connexion créée entre " + goo1.gameObject.name + " et " + goo2.gameObject.name);
        }
        else
        {
            Debug.Log("Les Goos sont trop éloignés pour se connecter.");
        }
    }


    private void CreateLineRenderer(Attach_Goo goo1, Attach_Goo goo2)
    {
        GameObject _line = Instantiate(LinePreafab, goo1.transform.position, Quaternion.Euler(0, 0, 0),
            goo1.transform);
        LineRenderer _linerender = _line.GetComponent<LineRenderer>();

        _linerender.SetPosition(0, goo1.gameObject.transform.position);
        _linerender.SetPosition(1, goo2.gameObject.transform.position);

        RbConnected.Add(goo2.gameObject);
        LinePreafabList.Add(_line);
    }

    private void Update()
    {
        for (int i = 0; i < LinePreafabList.Count; i++)
        {
            LinePreafabList[i].GetComponent<LineRenderer>()
                .SetPosition(0, LinePreafabList[i].transform.parent.position);
            LinePreafabList[i].GetComponent<LineRenderer>().SetPosition(1, RbConnected[i].transform.position);
        }

        for (int i = 0; i < springJoints.Length; i++)
        {
            if (!springJoints[i].isActiveAndEnabled)
            {
                Debug.Log("Game Over");
            }
        }
    }
}