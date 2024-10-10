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

    public void SetGooJoint(GameObject _gameObject, SpringJoint2D _jointUse)
    {
        List<GameObject> _goo = new List<GameObject>();
        _goo.Add(_gameObject);

        if (Goo1 == null)
        {
            Goo1 = Goo[_gameObject.name];
            Debug.Log(Goo1.transform.position);
        }
        else if (_gameObject != _goo[0] && Goo2 == null)
        {
            Goo2 = Goo[_gameObject.name];
            Debug.Log(Goo2.transform.position);
        }

        if (Goo1 != null && Goo2 != null)
        {
            _jointUse.connectedBody = Goo2.GetComponent<Rigidbody2D>();
            _jointUse.connectedAnchor = Vector2.zero;
            _goo.Clear();
            Goo1.GetComponent<LineRenderer>().SetPosition(1, Goo2.transform.position);
            Goo1 = null;
            Goo2 = null;
        }
    }
}