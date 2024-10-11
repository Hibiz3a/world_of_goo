using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attach_Goo : MonoBehaviour, IPointerClickHandler
{
    private SpringJoint2D Sj;
    private CircleCollider2D Cc;

    [SerializeField] private int RadiusDetection;
    [SerializeField] private SpringJoint2D[] springJoints;
    private LineRenderer LineTrace;


    public static Attach_Goo selectedGoo1;
    private Rigidbody2D rb2d;
    private int JointCount;
    private List<LineRenderer> activeLines = new List<LineRenderer>();


    private GameObject AttachGoo;

    private RaycastHit2D[] hit;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        springJoints = GetComponents<SpringJoint2D>();
        if (this.GetComponent<LineRenderer>())
        {
            LineTrace = GetComponent<LineRenderer>();
            LineTrace.startWidth = 0.1f;
            LineTrace.endWidth = 0.1f;
        }

        Sj = gameObject.GetComponent<SpringJoint2D>();
        Cc = gameObject.GetComponent<CircleCollider2D>();
    }


    private bool DetectGooInRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position, RadiusDetection, Vector2.zero,
            RadiusDetection);
        hit = hits;
        if (hits.Length > 0)
        {
            for (int i = 0; i <= hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Goo"))
                {
                    Debug.Log("true");
                    return true;
                }
            }
        }

        return false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Goo") && DetectGooInRange())
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
        SpringJoint2D availableJoint = null;
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


            Debug.Log("Connexion créée entre " + goo1.gameObject.name + " et " + goo2.gameObject.name);
        }
    }

    private void CreateLineRenderer(Attach_Goo goo1, Attach_Goo goo2)
    {
        switch (JointCount)
        {
            case 0:
                goo1.LineTrace.positionCount = 4;
                goo1.LineTrace.SetPosition(0, goo1.transform.position);
                goo1.LineTrace.SetPosition(1, goo2.transform.position);
                break;
            case 1:
                goo1.LineTrace.positionCount = goo1.LineTrace.positionCount + 2;
                goo1.LineTrace.SetPosition(2, goo1.transform.position);
                goo1.LineTrace.SetPosition(3, goo2.transform.position);
                break;
        }
    }


    private void Update()
    {
        if (springJoints[0].connectedBody != null)
        {
            for (int i = 0; i < springJoints.Length; i++)
            {
                SpringJoint2D joint = springJoints[i];
                if (joint.connectedBody != null)
                {
                    LineTrace.SetPosition(0, transform.position);
                    LineTrace.SetPosition(1, joint.connectedBody.transform.position);
                    if (joint == springJoints[1])
                    {
                        LineTrace.SetPosition(2, transform.position);
                        LineTrace.SetPosition(3, joint.connectedBody.transform.position);
                    }
                }
            }
        }
    }
}