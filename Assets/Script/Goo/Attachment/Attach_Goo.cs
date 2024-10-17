using System;
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

    //Bloc The connexion between 2 StartGoo
    if (goo1.isGooStart && goo2.isGooStart)
    {
        Debug.Log("Cannot connect two Goo Starts together.");
        return;
    }
    

    float distance = Vector2.Distance(goo1.transform.position, goo2.transform.position);

    //look is the selected Goo is in range
    if (distance <= connectionRange)
    {
        SpringJoint2D availableJoint = null;
        
        //EndGoo create automatically a Joint between him and the selected Goo
        if (goo1.isGooEnd || goo2.isGooEnd)
        {
            //If none of Goo is an EndGoo Initiate them  cancel
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
                CreateLineRenderer(goo2, goo1);
                availableJoint.connectedBody = goo1.rb2d;
                availableJoint.autoConfigureConnectedAnchor = false;
                availableJoint.connectedAnchor = Vector2.zero;
                goo1.JointCount++;
            }
        }

        //If Goo1 is a start or end Goo he can connect him to an Classic goo
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
            }
        }
        // Made the same but in other order
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
            //Goo1 and Goo2 is not Start or End Goo
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

        Debug.Log("Connexion between " + goo1.gameObject.name + " and " + goo2.gameObject.name);
    }
    else
    {
        Debug.Log("Goo's are too far from the other.");
    }
}



    private void CreateLineRenderer(Attach_Goo goo1, Attach_Goo goo2)
    {
    //Creation of a Game object with de the component LineRenderer
        GameObject _line = Instantiate(LinePreafab, goo1.transform.position, Quaternion.Euler(0, 0, 0),
            goo1.transform);
        LineRenderer _linerender = _line.GetComponent<LineRenderer>();

        //Set the position of Goo1 and Goo2 for the Start and the End of Line Render
        _linerender.SetPosition(0, goo1.gameObject.transform.position);
        _linerender.SetPosition(1, goo2.gameObject.transform.position);

        //Add Rigidbody ans LineRenderer to a list
        RbConnected.Add(goo2.gameObject);
        LinePreafabList.Add(_line);
    }

    private void OnDestroy()
    {
        // Clear the lists for line renderers and connected rigidbodies
        LinePreafabList.Clear();
        RbConnected.Clear();

        // Destroy all child objects attached to the Goo
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


    private void Update()
    {
        if (gameObject.activeSelf)
        {
            //Actualisation of LineRenderer.
            for (int i = 0; i < LinePreafabList.Count; i++)
            {
                LinePreafabList[i].GetComponent<LineRenderer>()
                    .SetPosition(0, LinePreafabList[i].transform.parent.position);
                LinePreafabList[i].GetComponent<LineRenderer>().SetPosition(1, RbConnected[i].transform.position);
            }

            //Detect if Goo as an SpringJoint Rupture.
            for (int i = 0; i < springJoints.Length; i++)
            {
                if (!springJoints[i].isActiveAndEnabled)
                {
                    Debug.Log("Game Over");
                }
            }
        }
    }
}