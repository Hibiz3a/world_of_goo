using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GooManager : MonoBehaviour
{
    private Dictionary<String, GameObject> Goo = new Dictionary<string, GameObject>();



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

}