using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attach_Goo : MonoBehaviour, IPointerClickHandler
{
    private SpringJoint2D Sj;
    private Dictionary<String,GameObject> GooInRange = new Dictionary<string, GameObject>();
    private CircleCollider2D Cc;

    private void Start()
    {
        Sj = gameObject.GetComponent<SpringJoint2D>();
        Cc = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goo"))
        {
            GooInRange.Add(other.name, other.gameObject);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (CompareTag("Goo") && Cc.isTrigger)
        {
            GooManager.instance.SetGooJoint(gameObject);
        }
    }
}