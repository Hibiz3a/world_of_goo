using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GooManager : MonoBehaviour
{
    private Dictionary<String, GameObject> Goo = new Dictionary<string, GameObject>();

    private GameObject Goo1 = null;
    private GameObject Goo2 = null;


    public static GooManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Goo.Add(gameObject.transform.GetChild(i).name, gameObject.transform.GetChild(i).gameObject);
        }
    }

    public void SetGooJoint(GameObject _gameObject)
    {
        if (Goo1 == null && Goo2 == null)
        {
            Goo1 = Goo[_gameObject.name];
            Debug.Log(Goo1.transform.position);
        }
        else if (Goo1 != null && Goo2 == null)
        {
            Goo2 = Goo[_gameObject.name];
            Debug.Log(Goo2.transform.position);
        }

        if (Goo1 != null && Goo2 != null)
        {
            Goo1.GetComponent<SpringJoint2D>().connectedBody = Goo2.GetComponent<Rigidbody2D>();
            Goo1.GetComponent<SpringJoint2D>().connectedAnchor = Goo2.transform.localPosition;
            Goo1 = null;
            Goo2 = null;
        }
    }
}