using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnd : MonoBehaviour
{

    
    
    private void OnCollisionEnter(Collision other)
    {
        if (CompareTag("Goo"))
        {
            transform.parent.GetComponent<Level_Manger>().EndLevelLoose();
        }
    }
}
